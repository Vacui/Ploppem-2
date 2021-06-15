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

    protected override void OnCreate() {
        DOTS_GameHandler.Instance.OnGameStarted += Reset;
    }

    protected override void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= Reset;
    }

    private void Reset(object sender, EventArgs args) {
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

        spawnTime = spawnData.SpawnFrequencyCurve.Evaluate(gameSessionTime);

        SpawnEnemy(new float3(
                UnityEngine.Random.Range(spawnData.SpawnLimitLeft, spawnData.SpawnLimitRight),
                UnityEngine.Random.Range(spawnData.SpawnLimitBottom, spawnData.SpawnLimitTop),
                0),
                spawnData.DirectionChangeFrequencyCurve.Evaluate(gameSessionTime),
                spawnData.MoveSpeedCurve.Evaluate(gameSessionTime),
                spawnData.SpawnLimitTop,
                spawnData.SpawnLimitRight,
                spawnData.SpawnLimitBottom,
                spawnData.SpawnLimitLeft,
                spawnData.LifetimeCurve.Evaluate(gameSessionTime),
                spawnData.DeathDuration,
                spawnData.DeathColor.ToFloat4()
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

    private void SpawnEnemy(float3 worldPosition, float directionChangeFrequency, float moveSpeed, float moveLimitTop, float moveLimitRight, float moveLimitBottom, float moveLimitLeft, float lifetime, float deathDuration, float4 deathColor) {

        EntityArchetype enemyEntityArchetype = EntityManager.CreateArchetype(
            typeof(Tag_GameSession),
            typeof(Tag_Enemy),
            typeof(Translation),
            typeof(MoveDirection),
            typeof(MoveDirectionChangeTimer),
            typeof(MoveSpeed),
            typeof(MoveLimits),
            typeof(Lifetime),
            typeof(DeathAnimationData),
            typeof(RenderingData)
            );

        Entity spawnedEntity = EntityManager.CreateEntity(enemyEntityArchetype);

        EntityManager.SetComponentData(spawnedEntity, new Translation {
            Value = worldPosition
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveDirectionChangeTimer {
            StartValue = directionChangeFrequency
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveSpeed {
            Value = moveSpeed
        });

        EntityManager.SetComponentData(spawnedEntity, new MoveLimits {
            Top = moveLimitTop,
            Right = moveLimitRight,
            Bottom = moveLimitBottom,
            Left = moveLimitLeft
        });

        EntityManager.SetComponentData(spawnedEntity, new Lifetime {
            Duration = lifetime
        });

        EntityManager.SetComponentData(spawnedEntity, new DeathAnimationData {
            Duration = deathDuration
        });

        EntityManager.SetComponentData(spawnedEntity, new RenderingData {
            SampledGradientReference = sampledColorGradientReference,
            DeathColor = deathColor
        });

    }

}