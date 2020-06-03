using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Entities;


[AlwaysSynchronizeSystem] //Засинкать тут же все изменения "на горячую"
public class MoveSystem : JobComponentSystem
{
    //из JobComponentSystem штука, чтоб обновлять значения в Component'е
    protected override JobHandle OnUpdate(JobHandle inputDeps) 
    {
        float deltaTime = Time.DeltaTime;
        //переменная, фиксирующая нажатия wasd
        float2 curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //перебор всех entity, которые содержит vel из MoveComponent
        Entities.ForEach((ref PhysicsVelocity vel, in MoveComponent moveComponent) =>
        {   
            //Текущие значения координат
            float2 newVel = vel.Linear.xz; 
            //Новые значения координат
            newVel += curInput * moveComponent.speed * deltaTime; 
            //Обновление координат
            vel.Linear.xz = newVel; 
        }).Run();

        return default;
    }


}


