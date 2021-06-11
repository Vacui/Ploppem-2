using Unity.Entities;

public struct MoveDirectionChangeTimer : IComponentData {
    public float StartValue;
    public float Value;
}