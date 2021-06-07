using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(LifetimeJobSystem))]
public class EnemyRenderingJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref LifetimeRenderingData lifetimeRenderingData, in Translation translation, in LifetimeComponent lifetime) => {

                // calculate layer, using just layers section 10 - 20
                lifetimeRenderingData.Layer = (int)math.floor((lifetime.Value / lifetime.Start) * (20 - 10)) + 10;

                lifetimeRenderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one);

            }).Schedule(inputDeps);

    }
}