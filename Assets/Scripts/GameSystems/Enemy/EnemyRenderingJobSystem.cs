using Unity.Jobs;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(LifetimeJobSystem))]
public class EnemyRenderingJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref LifetimeRenderingData lifetimeRenderingData, in Translation translation, in LifetimeComponent lifetime) => {

                lifetimeRenderingData.CurrentColor = lifetimeRenderingData.SampledGradientReference.Value.Evaluate(1f - lifetime.Value / lifetime.Start);
                lifetimeRenderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one);

            }).Schedule(inputDeps);

    }
}