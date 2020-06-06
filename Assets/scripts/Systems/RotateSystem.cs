using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;


[AlwaysSynchronizeSystem] //Засинкать тут же все изменения "на горячую"
public class RotateSystem : JobComponentSystem
{
    //из JobComponentSystem штука, чтоб обновлять значения в Component'е
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        //перебор всех entity, которые содержит vel из RotateComponent
        Entities.ForEach((ref Rotation rotation, in RotateComponent rotateSpeed) =>
        {
            rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(rotateSpeed.speed + deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(rotateSpeed.speed + deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(rotateSpeed.speed + deltaTime)));
        }).Run();

        return default;
    }
}


