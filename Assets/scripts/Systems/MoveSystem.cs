using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[AlwaysSynchronizeSystem] //Засинкать тут же все изменения "на горячую"
public class MoveSystem : JobComponentSystem
{
    //из JobComponentSystem штука, чтоб обновлять значения в Component'е
    protected override JobHandle OnUpdate(JobHandle inputDeps) 
    {
        float deltaTime = Time.DeltaTime;
        //переменная, фиксирующая нажатия wasd
        //перебор всех entity, которые содержит vel из MoveComponent
        Entities.ForEach((ref Rotation rotation, ref PhysicsVelocity vel, in MoveComponent moveComponent) =>
        {
            float3 forwardVector = math.mul(rotation.Value, new float3(0, 0, 1));
            vel.Linear.xyz += forwardVector * deltaTime * moveComponent.speed;
            

        }).Run();

        return default;
    }


}


