using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnerSystem : ComponentSystem {

    protected override void OnUpdate() {

        if (!HasSingleton<GameState>() ||
            GetSingleton<GameState>().Value != GameState.State.Playing) {
            return;
        }

        Entities
            .ForEach((ref EnemySpawnerComponent enemySpawner) => {

                enemySpawner.SpawnTime -= Time.DeltaTime;

                if (enemySpawner.SpawnTime > 0f) {
                    return;
                }

                if (!enemySpawner.Reference.IsCreated) {
                    return;
                }

                ref EnemySpawnDataBlobAsset spawnData = ref enemySpawner.Reference.Value;

                enemySpawner.SpawnTime = spawnData.SpawnFrequency;

                SpawnEnemy(new float3(
                        UnityEngine.Random.Range(spawnData.SpawnLimitLeft, spawnData.SpawnLimitRight),
                        UnityEngine.Random.Range(spawnData.SpawnLimitBottom, spawnData.SpawnLimitTop),
                        0),
                        ref spawnData);
                
            });

        return;
    }

    private void SpawnEnemy(float3 worldPosition, ref EnemySpawnDataBlobAsset spawnData) {

        EntityArchetype enemyEntityArchetype = EntityManager.CreateArchetype(
            typeof(GameSession),
            typeof(Enemy),
            typeof(Translation),
            typeof(DirectionComponent),
            typeof(DirectionChangeTimerComponent),
            typeof(MoveSpeedComponent),
            typeof(MoveLimitsComponent),
            typeof(LifetimeComponent),
            typeof(LifetimeRenderingData)
            );

        Entity spawnedEntity = EntityManager.CreateEntity(enemyEntityArchetype);

        EntityManager.SetComponentData(spawnedEntity, new Translation {
            Value = worldPosition
        });

        EntityManager.SetComponentData(spawnedEntity, new DirectionChangeTimerComponent {
            StartValue = spawnData.DirectionChangeFrequency
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveSpeedComponent {
            Value = spawnData.MoveSpeed
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveLimitsComponent {
            Top = spawnData.SpawnLimitTop,
            Right = spawnData.SpawnLimitRight,
            Bottom = spawnData.SpawnLimitBottom,
            Left = spawnData.SpawnLimitLeft
        });

        EntityManager.SetComponentData(spawnedEntity, new LifetimeComponent {
            Start = spawnData.Lifetime,
            Value = spawnData.Lifetime
        });

        EntityManager.SetComponentData(spawnedEntity, new LifetimeRenderingData {
            SampledGradientReference = spawnData.SampledGradientReference
        });

    }

}

public struct GameSession : IComponentData { }

public struct Enemy : IComponentData { }

public struct DirectionComponent : IComponentData {
    public float3 Value;
}

public struct DirectionChangeTimerComponent : IComponentData {
    public float StartValue;
    public float Value;
}

public struct MoveSpeedComponent : IComponentData {
    public float Value;
}

public struct MoveLimitsComponent : IComponentData {
    public float Top;
    public float Right;
    public float Bottom;
    public float Left;
}

public struct LifetimeComponent : IComponentData {
    public float Start;
    public float Value;
}

public struct LifetimeRenderingData : IComponentData {
    public float4 CurrentColor;
    public UnityEngine.Matrix4x4 Matrix;
    public BlobAssetReference<SampledGradientBlobAsset> SampledGradientReference;
}