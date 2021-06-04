﻿using Unity.Jobs;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(LifetimeJobSystem))]
public class LifetimeRenderingJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref LifetimeRenderingData lifetimeRenderingData, in LifetimeComponent lifetime) => {

                lifetimeRenderingData.CurrentColor = math.lerp(lifetimeRenderingData.StartColor, lifetimeRenderingData.EndColor, 1f - lifetime.Value / lifetime.Start);
                lifetimeRenderingData.Matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity, Vector3.one);

            }).Schedule(inputDeps);

    }
}