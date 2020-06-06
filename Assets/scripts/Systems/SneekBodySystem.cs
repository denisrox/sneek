﻿using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem] //Засинкать тут же все изменения "на горячую"
public class RotateSneekBodySystem : JobComponentSystem
{    
    // Start is called before the first frame update
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {          
        float deltaTime = Time.DeltaTime;
        //перебор всех entity с компонентом SneekBodyComponent для изменения их rotation
        Entities.ForEach((ref Rotation rotation, in SneekBodyComponent sneekBodyComponent) =>
        {            
            
            //targetEntity = GetComponentDataFromEntity<SneekBodyComponent>();        
            //if (rotation.Value != )    
            //rotation.Value = Quaternion.LookRotation();
            /*if())
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(playerRotateSpeed.speed * deltaTime)));
            if(Input.GetKey(playerRotateSpeed.left))
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(-math.radians(playerRotateSpeed.speed * deltaTime)));

            //rotation.Value = math.mul(rotation.Value, quaternion.RotateY(-math.radians(targetForBodyComponent.speed * deltaTime)));
            //Quaternion.LookRotation(target)
            //EntityManager manager;
            //Quaternion.LookRotation(manager.GetComponentData<Translation>(target.target));
*/
        }).Run();

        return default;
    }
}
