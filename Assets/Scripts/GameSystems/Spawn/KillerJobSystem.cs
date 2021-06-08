using Unity.Entities;
using Unity.Jobs;

public class KillerJobSystem : JobComponentSystem {

    private EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate() {
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        EntityCommandBuffer.Concurrent entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        JobHandle jobHandle = Entities
            .WithAll<DeathMark>()
            .ForEach((int nativeThreadIndex, Entity entity) => {

                entityCommandBuffer.DestroyEntity(nativeThreadIndex, entity);

            }).Schedule(inputDeps);

        entityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;

    }
}