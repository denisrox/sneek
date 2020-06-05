using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerRotateComponent : IComponentData
{
    public float speed;
    public KeyCode left;
    public KeyCode right;
}
