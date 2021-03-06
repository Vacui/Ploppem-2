using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Events;

public class LifetimeJobSystem : JobComponentSystem {

    public event UnityAction OnEnemyDead;
    public struct OnEnemyDeadEvent : IComponentData { public int Value; }
    private DOTSEvents_NextFrame<OnEnemyDeadEvent> dotsEvents;

    private EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate() {
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        dotsEvents = new DOTSEvents_NextFrame<OnEnemyDeadEvent>(World);
    }

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        EntityCommandBuffer.Concurrent entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        DOTSEvents_NextFrame<OnEnemyDeadEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();

        JobHandle jobHandle = Entities
            .WithAll<Tag_Enemy>()
            .WithNone<Tag_DeathMark>()
            .ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => {
                lifetime.Value += deltaTime;

                if (lifetime.Value > lifetime.Duration) {
                    // Mark the Entity as Dead
                    entityCommandBuffer.AddComponent(entityInQueryIndex, entity, new Tag_DeathMark());
                    eventTrigger.TriggerEvent(entityInQueryIndex);
                }
            }).Schedule(inputDeps);

        entityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        dotsEvents.CaptureEvents(eventTrigger, jobHandle, (OnEnemyDeadEvent onEnemyDeadEvent) => {
            OnEnemyDead?.Invoke();
        });

        return jobHandle;
    }
}