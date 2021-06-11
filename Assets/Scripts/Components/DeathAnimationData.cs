using Unity.Entities;

public struct DeathAnimationData : IComponentData {
    public float Duration;
    public float Value;
    public bool Killed;
}