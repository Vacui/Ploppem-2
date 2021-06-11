using Unity.Entities;

public struct MoveLimits : IComponentData {
    public float Top;
    public float Right;
    public float Bottom;
    public float Left;
}