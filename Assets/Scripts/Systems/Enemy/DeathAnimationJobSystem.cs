using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class DeathAnimationJobSystem : JobComponentSystem {

    private EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate() {
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        EntityCommandBuffer.Concurrent entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        JobHandle jobHandle = Entities
            .WithAll<Tag_Enemy, Tag_DeathMark>()
            .ForEach((Entity entity, int entityInQueryIndex, ref DeathAnimationData deathAnimData) => {

                deathAnimData.Value += deltaTime;

                if (deathAnimData.Value > deathAnimData.Duration) {
                    // Destroy Entity
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }

            }).Schedule(inputDeps);

        entityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}