using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnerJobSystem : JobComponentSystem {

    private float spawnTime;

    public void Reset() {
        spawnTime = 0f;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

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
            Value = spawnData.Lifetime
        });

    }

}

public struct Enemy : IComponentData { }