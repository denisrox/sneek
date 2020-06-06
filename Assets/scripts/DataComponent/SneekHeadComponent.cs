using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SneekHeadComponent : IComponentData
{
    public float speed;
    public KeyCode left;
    public KeyCode right;
}
