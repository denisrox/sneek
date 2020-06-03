using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using unityMath = Unity.Mathematics;

[AlwaysSynchronizeSystem]
//Это значит, что эта система отработает только тогда, когда отработает PickupSystem (а это триггер)
public class RandomSpawnSystem : JobComponentSystem
{
    //private RotateComponent component;
    //Для мультипоточности? Или чтоб результат работы всех коммандБафферов сохранился? Не понятно!
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate() 
    {
        //Если этой системы нет, то создать
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //Структура спавнера. В OnUpdate с её помощью создаются энтити по префабу
    private struct SpawnerJob : IJobForEachWithEntity<Spawner, LocalToWorld>
    {
        private EntityCommandBuffer.Concurrent entityCommandBuffer;
        private unityMath.Random random;

        public SpawnerJob(EntityCommandBuffer.Concurrent entityCommandBuffer, unityMath.Random random ) 
        {
            this.entityCommandBuffer = entityCommandBuffer;
            this.random = random;
        }

        public void Execute(Entity entity, int index, ref Spawner spawner, [ReadOnly] ref LocalToWorld localToWorld)
        {       
            Entity instance = entityCommandBuffer.Instantiate(index, spawner.prefab); 
            entityCommandBuffer.SetComponent(index, instance, new Translation
            {
                Value = new unityMath.float3(
                    random.NextFloat(-spawner.randomRange, spawner.randomRange) ,
                    1,
                    random.NextFloat(-spawner.randomRange, spawner.randomRange))
            });
        }
    }
    //Просто спавнить непрерывно энтити по префабу
    /*protected override JobHandle OnUpdate(JobHandle inputDeps)
    {        
        var SpawnerJob = new SpawnerJob(
            endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            new unityMath.Random((uint)UnityEngine.Random.Range(0,int.MaxValue))
        );

        JobHandle jobHandle = SpawnerJob.Schedule(this, inputDeps);

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }*/

    //Спавнить энтити по префабу, когда список объектов с DeleteTag не пустой (всратый, но рабочий пока что костыль, нужно переписывать!)
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {        
        bool trigger = false;
        var jobHandle = new JobHandle();
        Entities
            .WithAll<DeleteTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                var SpawnerJob = new SpawnerJob(
                    endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                    new unityMath.Random((uint)UnityEngine.Random.Range(0,int.MaxValue)));      
                            
                jobHandle = SpawnerJob.Schedule(this, inputDeps);

                endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);   
                trigger = true;     
            }).Run();        
        if(trigger) 
        {
            return jobHandle; 
        }
        else
        {
            return default;
        }
    }
}
