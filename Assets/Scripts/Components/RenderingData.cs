using Unity.Entities;
using Unity.Mathematics;

public struct RenderingData : IComponentData {
    public float4 Color;
    public int Layer;
    public UnityEngine.Matrix4x4 Matrix;
    public BlobAssetReference<SampledGradientBlobAsset> SampledGradientReference;
    public float4 DeathColor;
}