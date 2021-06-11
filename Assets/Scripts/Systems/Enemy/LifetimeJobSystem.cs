using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class LifetimeJobSystem : JobComponentSystem {

    private EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate() {
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        EntityCommandBuffer.Concurrent entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        JobHandle jobHandle = Entities
            .WithAll<Tag_Enemy>()
            .WithNone<Tag_DeathMark>()
            .ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => {
                lifetime.Value += deltaTime;

                if (lifetime.Value > lifetime.Duration) {
                    // Mark the Entity as Dead
                    entityCommandBuffer.AddComponent(entityInQueryIndex, entity, typeof(Tag_DeathMark));
                }
            }).Schedule(inputDeps);

        entityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}