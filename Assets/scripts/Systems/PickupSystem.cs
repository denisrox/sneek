using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;

public class PickupSystem : JobComponentSystem
{
    //Понятия не имею что это за штуки, но они нужны в OnUpdate и OnCreate
    private BeginInitializationEntityCommandBufferSystem bufferSystem;
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        //Нужно только лишь для commandBuffer в OnUpdate
        bufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        //Эти штуки нужны для Schedule в OnUpdate
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();        
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();        
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob triggerJob = new TriggerJob
        {
            speedEntities = GetComponentDataFromEntity<MoveComponent>(),
            entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(),
            commandBuffer = bufferSystem.CreateCommandBuffer()
        };
        
        JobHandle jobHandle = 
            triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();
        //ХЗ зачем шедулер и зачем ему такие параметры        
        return default;
    }

    private struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<MoveComponent> speedEntities;
        [ReadOnly] public ComponentDataFromEntity<DeleteTag> entitiesToDelete;
        public EntityCommandBuffer commandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            //Неизвестно, кто из двух будет триггером, поэтому нужно учесть оба сценария его активации
            TestEntityTrigger(triggerEvent.Entities.EntityA, triggerEvent.Entities.EntityB);
            TestEntityTrigger(triggerEvent.Entities.EntityB, triggerEvent.Entities.EntityA);
        }

        private void TestEntityTrigger(Entity entity1, Entity entity2)
        {
            //если объект имеет entity с компонентами из MoveComponent, то это EntityA
            if (speedEntities.HasComponent(entity1))
            {
                //если объект имеет entity с компонентами из DeleteTag, то это EntityB. Он нам нужен, возвращаем
                if (entitiesToDelete.HasComponent(entity2)) { return; }
                //Что такое commandBuffer?
                commandBuffer.AddComponent(entity2, new DeleteTag());                
            }
        }
    }
}
