using Unity.Jobs;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(LifetimeJobSystem))]
public class EnemyRenderingJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref LifetimeRenderingData lifetimeRenderingData, in Translation translation, in LifetimeComponent lifetime) => {

                float lifetimePercentage = 1f - lifetime.Value / lifetime.Start;

                lifetimeRenderingData.CurrentColor = lifetimeRenderingData.SampledGradientReference.Value.Evaluate(lifetimePercentage);

                // calculate layer, using just layers section 10 - 20
                lifetimeRenderingData.Layer = (int)math.floor((1f - lifetimePercentage) * (20 - 10)) + 10;

                lifetimeRenderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one);

            }).Schedule(inputDeps);

    }
}