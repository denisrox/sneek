using Unity.Entities;
using System.Collections.Generic;
using UnityEngine;

//Конвертация префаба из GameObject в Entity на лету
public class SpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] private GameObject prefab;
    
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) 
    {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Spawner
        {
            prefab = conversionSystem.GetPrimaryEntity(prefab)
        });
    }
}
