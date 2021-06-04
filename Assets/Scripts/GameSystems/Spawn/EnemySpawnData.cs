using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnData : MonoBehaviour {

    public static EnemySpawnData Instance { get; private set; }

    private EntityManager entityManger;

    [Header("Rendering")]
    [SerializeField] private Mesh mesh;
    public Mesh Mesh => mesh;
    [SerializeField] private Material material;
    public Material Material => material;

    [Header("Stats")]
    [SerializeField] private float spawnFrequency;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float directionChangeFrequency;
    [SerializeField] private float lifeTime;

    [Header("Limits")]
    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

    private float spawnLimitTop;
    private float spawnLimitRight;
    private float spawnLimitBottom;
    private float spawnLimitLeft;

    private Entity referenceHolderEntity;

    private void Awake() {

        Instance = this;

        entityManger = World.DefaultGameObjectInjectionWorld.EntityManager;

        CalculateSpawnLimits();
    }

    private void CalculateSpawnLimits() {

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        spawnLimitBottom = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        spawnLimitLeft = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        spawnLimitRight = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        spawnLimitTop = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);

    }

    private void Start() {
        DOTS_GameHandler.Instance.OnGameStarted += CreateEntitySpawnBlobAsset;
        DOTS_GameHandler.Instance.OnGameOver += DestroyBlobAssetReference;
    }

    private void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= CreateEntitySpawnBlobAsset;
        DOTS_GameHandler.Instance.OnGameOver -= DestroyBlobAssetReference;
    }

    private void CreateEntitySpawnBlobAsset(object sender, EventArgs args) {

        BlobAssetReference<EnemySpawnDataBlobAsset> entitySpawnDataBlobAssetReference;

        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp)) {
            ref EnemySpawnDataBlobAsset entitySpawnDataBlobAsset = ref blobBuilder.ConstructRoot<EnemySpawnDataBlobAsset>();

            entitySpawnDataBlobAsset.SpawnFrequency = spawnFrequency;
            entitySpawnDataBlobAsset.MoveSpeed = moveSpeed;
            entitySpawnDataBlobAsset.DirectionChangeFrequency = directionChangeFrequency;
            entitySpawnDataBlobAsset.Lifetime = lifeTime;

            entitySpawnDataBlobAsset.SpawnLimitTop = spawnLimitTop;
            entitySpawnDataBlobAsset.SpawnLimitRight = spawnLimitRight;
            entitySpawnDataBlobAsset.SpawnLimitBottom = spawnLimitBottom;
            entitySpawnDataBlobAsset.SpawnLimitLeft = spawnLimitLeft;

            entitySpawnDataBlobAssetReference = blobBuilder.CreateBlobAssetReference<EnemySpawnDataBlobAsset>(Allocator.Persistent);
        }

        referenceHolderEntity = entityManger.CreateEntity();

        entityManger.AddComponentData(referenceHolderEntity, new EnemySpawnDataBlobAssetReference {
            Reference = entitySpawnDataBlobAssetReference
        });

    }

    private void DestroyBlobAssetReference(object sender, EventArgs args) {
        entityManger.DestroyEntity(referenceHolderEntity);
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

    public float SpawnFrequency;
    public float MoveSpeed;
    public float DirectionChangeFrequency;
    public float Lifetime;

    public float SpawnLimitTop;
    public float SpawnLimitRight;
    public float SpawnLimitBottom;
    public float SpawnLimitLeft;

}

public struct EnemySpawnDataBlobAssetReference : IComponentData {

    public BlobAssetReference<EnemySpawnDataBlobAsset> Reference;

}