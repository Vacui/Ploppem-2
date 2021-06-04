using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnerSystem : ComponentSystem {

    private float spawnTime;

    public void Reset() {

        spawnTime = 0f;

    }

    protected override void OnUpdate() {

        if (!HasSingleton<GameState>() ||
            !(GetSingleton<GameState>().Value == GameState.State.Playing)) {
            return;
        }

        spawnTime -= Time.DeltaTime;

        if (spawnTime > 0f) {
            return;
        }

        if (!HasSingleton<EnemySpawnDataBlobAssetReference>() ||
            !GetSingleton<EnemySpawnDataBlobAssetReference>().Reference.IsCreated) {
            return;
        }

        EnemySpawnDataBlobAsset spawnData = GetSingleton<EnemySpawnDataBlobAssetReference>().Reference.Value;

        /*UnityEngine.Debug.Log($"" +
            $"{spawnData.SpawnFrequency}, " +
            $"{spawnData.MoveSpeed}, " +
            $"{spawnData.DirectionChangeFrequency}, " +
            $"{spawnData.Lifetime}, " +
            $"{spawnData.SpawnLimitTop}-{spawnData.SpawnLimitRight}-{spawnData.SpawnLimitBottom}-{spawnData.SpawnLimitLeft}"
            );*/

        spawnTime = spawnData.SpawnFrequency;

        SpawnEnemy(spawnData);

        return;

    }

    private void SpawnEnemy(EnemySpawnDataBlobAsset spawnData) {

        EntityArchetype enemyEntityArchetype = EntityManager.CreateArchetype(
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
            Value = new float3(
                UnityEngine.Random.Range(spawnData.SpawnLimitLeft, spawnData.SpawnLimitRight),
                UnityEngine.Random.Range(spawnData.SpawnLimitBottom, spawnData.SpawnLimitTop),
                0)
        });

        EntityManager.SetComponentData(spawnedEntity, new DirectionComponent { });

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
            StartColor = new float4(1, 1, 1, 1),
            EndColor = new float4(0, 0, 0, 1)
        });

    }

}

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
    public float4 StartColor;
    public float4 EndColor;
    public float4 CurrentColor;
    public UnityEngine.Matrix4x4 Matrix;
}