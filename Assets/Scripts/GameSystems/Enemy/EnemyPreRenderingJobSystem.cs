using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(LifetimeJobSystem))]
public class EnemyPreRenderingJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref EnemyRenderingData lifetimeRenderingData, in Translation translation, in LifetimeComponent lifetime, in DeathAnimationData deathAnimData) => {

                float remainingLifetimePercentage = lifetime.Value / lifetime.Start;

                // calculate layer, using just layers section 10 - 20
                lifetimeRenderingData.Layer = (int)math.floor(remainingLifetimePercentage * (20 - 10)) + 10;

                float scaleFactor = 1f;
                float deathAnimPercentage = deathAnimData.Value / deathAnimData.Duration;
                if (deathAnimPercentage > 0f) {
                    scaleFactor = math.clamp(1f - deathAnimPercentage, 0f, 1f);
                }

                lifetimeRenderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one * scaleFactor);

            }).Schedule(inputDeps);
    }
}