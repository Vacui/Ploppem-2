using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnerSystem : ComponentSystem {

    private float spawnTime;
    private float gameSessionTime;

    public void Reset() {
        spawnTime = 0f;
        gameSessionTime = 0f;
    }

    protected override void OnUpdate() {

        if (!HasSingleton<GameState>() ||
            GetSingleton<GameState>().Value != GameState.State.Playing) {
            return;
        }

        gameSessionTime += Time.DeltaTime;
        spawnTime -= Time.DeltaTime;

        if (spawnTime > 0f) {
            return;
        }

        EnemySpawnerData spawnData = EnemySpawnerData.Instance;

        if(spawnData == null) {
            return;
        }

        spawnTime = spawnData.SpawnFrequency;

        SpawnEnemy(new float3(
                UnityEngine.Random.Range(spawnData.SpawnLimitLeft, spawnData.SpawnLimitRight),
                UnityEngine.Random.Range(spawnData.SpawnLimitBottom, spawnData.SpawnLimitTop),
                0),
                spawnData.DirectionChangeFrequency,
                spawnData.MoveSpeedCurve.Evaluate(gameSessionTime),
                spawnData.SpawnLimitTop,
                spawnData.SpawnLimitRight,
                spawnData.SpawnLimitBottom,
                spawnData.SpawnLimitLeft,
                spawnData.Lifetime,
                spawnData.DeathDuration
                );

        return;
    }

    private void SpawnEnemy(float3 worldPosition, float directionChangeFrequency, float moveSpeed, float moveLimitTop, float moveLimitRight, float moveLimitBottom, float moveLimitLeft, float lifetime, float deathDuration) {

        EntityArchetype enemyEntityArchetype = EntityManager.CreateArchetype(
            typeof(GameSession),
            typeof(Enemy),
            typeof(Translation),
            typeof(DirectionComponent),
            typeof(DirectionChangeTimerComponent),
            typeof(MoveSpeedComponent),
            typeof(MoveLimitsComponent),
            typeof(LifetimeComponent),
            typeof(DeathAnimationData),
            typeof(EnemyRenderingData)
            );

        Entity spawnedEntity = EntityManager.CreateEntity(enemyEntityArchetype);

        EntityManager.SetComponentData(spawnedEntity, new Translation {
            Value = worldPosition
        });

        EntityManager.SetComponentData(spawnedEntity, new DirectionChangeTimerComponent {
            StartValue = directionChangeFrequency
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveSpeedComponent {
            Value = moveSpeed
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveLimitsComponent {
            Top = moveLimitTop,
            Right = moveLimitRight,
            Bottom = moveLimitBottom,
            Left = moveLimitLeft
        });

        EntityManager.SetComponentData(spawnedEntity, new LifetimeComponent {
            Start = lifetime,
            Value = lifetime
        });

        EntityManager.SetComponentData(spawnedEntity, new DeathAnimationData {
            Duration = deathDuration
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

public struct DeathAnimationData : IComponentData {
    public float Duration;
    public float Value;
}

public struct EnemyRenderingData : IComponentData {
    public int Layer;
    public float Scale;
    public UnityEngine.Matrix4x4 Matrix;
    public BlobAssetReference<SampledGradientBlobAsset> SampledGradientReference;
}