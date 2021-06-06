using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utils;

public class EnemySpawnData : MonoBehaviour {

    public static EnemySpawnData Instance { get; private set; }

    private EntityManager entityManger;

    [Header("Rendering")]
    [SerializeField] private Mesh mesh;
    public Mesh Mesh => mesh;
    [SerializeField] private Material material;
    public Material Material => material;
    [SerializeField] private Gradient colorGradient;

    [Header("Stats")]
    [SerializeField] private float spawnFrequency;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float directionChangeFrequency;
    [SerializeField] private float lifetime;

    [Header("Limits")]
    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

    private float spawnLimitTop;
    private float spawnLimitRight;
    private float spawnLimitBottom;
    private float spawnLimitLeft;

    private void Awake() {

        Instance = this;

        entityManger = World.DefaultGameObjectInjectionWorld.EntityManager;

        CalculateSpawnLimits();
    }

    private void Start() {
        DOTS_GameHandler.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= OnGameStarted;
    }

    private void CalculateSpawnLimits() {

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        spawnLimitBottom = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        spawnLimitLeft = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        spawnLimitRight = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        spawnLimitTop = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);

    }

    private void OnGameStarted(object sender, EventArgs args) {
        CalculateSpawnLimits();
        CreateEnemySpawnerEntity();
    }

    private void CreateEnemySpawnerEntity() {

        NativeArray<float4> sampledGradient = UtilsClass.SampleGradient(colorGradient, 1000, Allocator.Temp);
        BlobAssetReference<SampledGradientBlobAsset> sampledGradientBlobAssetReference = BlobAssetUtils.BuildBlobAsset(sampledGradient, delegate (ref SampledGradientBlobAsset blobAsset, NativeArray<float4> data) {

            BlobBuilder blobBuilder = BlobAssetUtils.BlobBuilder;

            BlobBuilderArray<float4> sampledGradientArrayBuilder = blobBuilder.Allocate(ref blobAsset.SampledGradient, data.Length);

            for (int i = 0; i < data.Length; i++) {
                sampledGradientArrayBuilder[i] = data[i];
            }

        });
        sampledGradient.Dispose();

        BlobAssetReference<EnemySpawnDataBlobAsset> entitySpawnDataBlobAssetReference = BlobAssetUtils.BuildBlobAsset(this, delegate (ref EnemySpawnDataBlobAsset blobAsset, EnemySpawnData data) {

            blobAsset.SampledGradientReference = sampledGradientBlobAssetReference;

            blobAsset.SpawnFrequency = spawnFrequency;
            blobAsset.MoveSpeed = moveSpeed;
            blobAsset.DirectionChangeFrequency = directionChangeFrequency;
            blobAsset.Lifetime = lifetime;

            blobAsset.SpawnLimitTop = spawnLimitTop;
            blobAsset.SpawnLimitRight = spawnLimitRight;
            blobAsset.SpawnLimitBottom = spawnLimitBottom;
            blobAsset.SpawnLimitLeft = spawnLimitLeft;

        });

        Entity enemySpawnerEntity = entityManger.CreateEntity();

        entityManger.AddComponentData(enemySpawnerEntity, new GameSession { });

        entityManger.AddComponentData(enemySpawnerEntity, new EnemySpawnerComponent {
            Reference = entitySpawnDataBlobAssetReference
        });

    }

    private void OnDrawGizmosSelected() {

        CalculateSpawnLimits();

        Gizmos.DrawLine(new Vector3(spawnLimitLeft, spawnLimitBottom), new Vector3(spawnLimitRight, spawnLimitBottom));
        Gizmos.DrawLine(new Vector3(spawnLimitRight, spawnLimitBottom), new Vector3(spawnLimitRight, spawnLimitTop));
        Gizmos.DrawLine(new Vector3(spawnLimitRight, spawnLimitTop), new Vector3(spawnLimitLeft, spawnLimitTop));
        Gizmos.DrawLine(new Vector3(spawnLimitLeft, spawnLimitTop), new Vector3(spawnLimitLeft, spawnLimitBottom));

    }

}

public struct EnemySpawnDataBlobAsset {
    //public BlobAssetReference<AnimationCurveBlobAsset> AnimationCurveReference;
    public BlobAssetReference<SampledGradientBlobAsset> SampledGradientReference;
    public float SpawnFrequency;
    public float MoveSpeed;
    public float DirectionChangeFrequency;
    public float Lifetime;

    public float SpawnLimitTop;
    public float SpawnLimitRight;
    public float SpawnLimitBottom;
    public float SpawnLimitLeft;

}

public struct AnimationCurveBlobAsset {
    public BlobArray<float> SampledAnimationCurve;
    public float Min;
    public float Max;

    /// <param name="time">Must be between Min and Max</param>
    public float Evaluate(float time) {
        int lenght = SampledAnimationCurve.Length - 1;
        time = math.clamp(time, Min, Max);
        float time01 = time / (Max - Min);
        float floatIndex = (time01 * lenght);
        int floorIndex = (int)math.floor(floatIndex);
        return SampledAnimationCurve[floorIndex];
    }
}

public struct SampledGradientBlobAsset {

    public BlobArray<float4> SampledGradient;

    /// <param name="time">Must be from 0 to 1</param>
    public float4 Evaluate(float time) {
        int length = SampledGradient.Length - 1;
        time = math.clamp(time, 0, 1);
        float floatIndex = (time * length);
        int floorIndex = (int)math.floor(floatIndex);
        return SampledGradient[floorIndex];

    }

}

public struct EnemySpawnerComponent : IComponentData {
    public BlobAssetReference<EnemySpawnDataBlobAsset> Reference;
    public float SpawnTime;
}