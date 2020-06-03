using Unity.Entities;

[GenerateAuthoringComponent]
public struct Spawner : IComponentData
{
    public Entity prefab;
    public int randomRange;
}
