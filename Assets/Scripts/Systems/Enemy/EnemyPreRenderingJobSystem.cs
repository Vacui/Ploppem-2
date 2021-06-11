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
            .ForEach((ref EnemyRenderingData renderingData, in Translation translation, in LifetimeComponent lifetime, in DeathAnimationData deathAnimData) => {

                float lifetimePercentage = lifetime.Value / lifetime.Duration;
                float deathAnimPercentage = deathAnimData.Value / deathAnimData.Duration;

                float scaleFactor = 1f;
                float4 color = renderingData.SampledGradientReference.Value.Evaluate(lifetimePercentage);
                if (deathAnimPercentage > 0f) {
                    if (deathAnimData.Killed) {
                        scaleFactor = math.clamp(deathAnimPercentage + 1f, 1f, 2f);
                        color.w = math.clamp(1f - deathAnimPercentage, 0f, 1f);
                    } else {
                        scaleFactor = math.clamp(1f - deathAnimPercentage, 0f, 1f);
                        color = renderingData.DeathColor;
                    }
                }
                renderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one * scaleFactor);

                renderingData.Color = color;

                // calculate layer, using just layers section 10 - 20
                renderingData.Layer = (int)math.floor((1f - lifetimePercentage) * (20 - 10)) + 10;


            }).Schedule(inputDeps);
    }
}