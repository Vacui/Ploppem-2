using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class EnemySpawnerJobSystem : JobComponentSystem {

    private float spawnTime;

    public void Reset() {
        spawnTime = 0f;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        if (!HasSingleton<GameState>() ||
            GetSingleton<GameState>().Value != GameState.State.Playing) {
            return default;
        }

        spawnTime -= Time.DeltaTime;

        if (spawnTime > 0f) {
            return default;
        }

        if (!HasSingleton<EnemySpawnDataBlobAssetReference>() ||
           !GetSingleton<EnemySpawnDataBlobAssetReference>().Reference.IsCreated) {
            return default;
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

        return default;

    }

    private void SpawnEnemy(EnemySpawnDataBlobAsset spawnData) {

        Entity prefabEntity = GetSingleton<PrefabEntityComponent>().Prefab;

        Entity spawnedEntity = EntityManager.Instantiate(prefabEntity);

        EntityManager.AddComponent(spawnedEntity, typeof(Enemy));

        /*
        EntityManager.SetSharedComponentData(spawnedEntity, new RenderMesh {
            mesh = entityMesh,
            material = entityMaterial,
        });
        */

        EntityManager.RemoveComponent(spawnedEntity, typeof(RenderMesh));
        EntityManager.RemoveComponent(spawnedEntity, typeof(RenderBounds));

        EntityManager.SetComponentData(spawnedEntity, new Translation {
            Value = new float3(
                UnityEngine.Random.Range(spawnData.SpawnLimitLeft, spawnData.SpawnLimitRight),
                UnityEngine.Random.Range(spawnData.SpawnLimitBottom, spawnData.SpawnLimitTop),
                0)
        });

        EntityManager.AddComponentData(spawnedEntity, new DirectionComponent { });

        EntityManager.AddComponentData(spawnedEntity, new DirectionChangeTimerComponent {
            StartValue = spawnData.DirectionChangeFrequency
        });

        EntityManager.AddComponentData(spawnedEntity, new MoveSpeedComponent {
            Value = spawnData.MoveSpeed
        });

        EntityManager.AddComponentData(spawnedEntity, new MoveLimitsComponent {
            Top = spawnData.SpawnLimitTop,
            Right = spawnData.SpawnLimitRight,
            Bottom = spawnData.SpawnLimitBottom,
            Left = spawnData.SpawnLimitLeft
        });

        EntityManager.AddComponentData(spawnedEntity, new LifetimeComponent {
            Start = spawnData.Lifetime,
            Value = spawnData.Lifetime
        });

        EntityManager.AddComponentData(spawnedEntity, new LifetimeRenderingData {
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
    public Matrix4x4 Matrix;
}