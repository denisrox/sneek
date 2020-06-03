using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;

[AlwaysSynchronizeSystem]
//Это значит, что эта система отработает только тогда, когда отработает PickupSystem (а это триггер)
[UpdateAfter(typeof(PickupSystem))]
[UpdateAfter(typeof(RandomSpawnSystem))]
public class DeleteEntitySystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities
            .WithAll<DeleteTag>()
            .WithoutBurst() //с ним нельзя почему-то GameManager использовать
            .ForEach((Entity entity) =>
            {
                GameManager.instanse.IncreaseScore();
                commandBuffer.DestroyEntity(entity);                
            }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}
