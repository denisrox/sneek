using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instanse; //почему именно инстанс?

    public Entity ballEntity;
    public float3 offset;

    private EntityManager manager;

    private void Awake()
    {
        if (instanse != null && instanse != this)
        {
            Destroy(gameObject); //зачем дестроить?
            return;
        }

        instanse = this;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    //Апдейт после каждого фрейма (энтити по фреймам "тикает")
    private void LateUpdate()
    {
        if (ballEntity == null) { return; } //Уйти отсюда, если мяча нет

        Translation ballPos = manager.GetComponentData<Translation>(ballEntity);
        transform.position = ballPos.Value + offset;
    }
}
