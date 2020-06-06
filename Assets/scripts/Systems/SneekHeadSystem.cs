using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem] //Засинкать тут же все изменения "на горячую"
public class SneekHeadSystem : JobComponentSystem
{
    // Start is called before the first frame update
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        //перебор всех entity, которые содержит vel из RotateComponent
        Entities.ForEach((ref Rotation rotation, in SneekHeadComponent playerRotateSpeed) =>
        {
            if(Input.GetKey(playerRotateSpeed.right))
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(playerRotateSpeed.speed * deltaTime)));
            if(Input.GetKey(playerRotateSpeed.left))
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(-math.radians(playerRotateSpeed.speed * deltaTime)));
            
        }).Run();

        return default;
    }
}






