﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class LifetimeJobSystem : JobComponentSystem {

    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBuffer;

    protected override void OnCreate() {
        endSimulationEntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        EntityCommandBuffer.Concurrent entityCommandBuffer = endSimulationEntityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        JobHandle jobHandle = Entities
            .WithAll<Enemy>()
            .ForEach((Entity entity, int entityInQueryIndex, ref LifetimeComponent lifetime) => {
                lifetime.Value -= deltaTime;

                if (lifetime.Value <= 0f) {
                    // Destroy Entity
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }
            }).Schedule(inputDeps);

        endSimulationEntityCommandBuffer.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}