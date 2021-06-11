using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Utils;

public class EnemySpawnerSystem : ComponentSystem {

    private float spawnTime;
    private float gameSessionTime;

    private BlobAssetReference<SampledGradientBlobAsset> sampledColorGradientReference;

    public void Reset() {
        spawnTime = 0f;
        gameSessionTime = 0f;

        if (sampledColorGradientReference.IsCreated) {
            sampledColorGradientReference.Dispose();
        }
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

        if (!sampledColorGradientReference.IsCreated) {
            SampleColorGradient(spawnData.ColorGradient);
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

    private void SampleColorGradient(Gradient colorGradient) {

        NativeArray<float4> sampledGradient = UtilsClass.SampleGradient(colorGradient, 1000, Allocator.Temp);
        sampledColorGradientReference = BlobAssetUtils.BuildBlobAsset(sampledGradient, delegate (ref SampledGradientBlobAsset blobAsset, NativeArray<float4> data) {

            BlobBuilder blobBuilder = BlobAssetUtils.BlobBuilder;

            BlobBuilderArray<float4> sampledGradientArrayBuilder = blobBuilder.Allocate(ref blobAsset.SampledGradient, data.Length);

            for (int i = 0; i < data.Length; i++) {
                sampledGradientArrayBuilder[i] = data[i];
            }

        });
        sampledGradient.Dispose();

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
            Duration = lifetime
        });

        EntityManager.SetComponentData(spawnedEntity, new DeathAnimationData {
            Duration = deathDuration
        });

        EntityManager.SetComponentData(spawnedEntity, new EnemyRenderingData {
            SampledGradientReference = sampledColorGradientReference
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
    public float Duration;
    public float Value;
}

public struct DeathAnimationData : IComponentData {
    public float Duration;
    public float Value;
}

public struct EnemyRenderingData : IComponentData {
    public float4 Color;
    public int Layer;
    public UnityEngine.Matrix4x4 Matrix;
    public BlobAssetReference<SampledGradientBlobAsset> SampledGradientReference;
}