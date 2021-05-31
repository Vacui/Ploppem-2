using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Enemy : IComponentData { }

public struct DirectionComponent : IComponentData {
    public float3 Value;
}

public struct MoveSpeedComponent : IComponentData {
    public float Value;
}

public struct MoveLimitsComponent : IComponentData {
    public float Top;
    public float Right;
    public float Bottom;
    public float Left;
}

/*
public class MoveJobSystem : JobComponentSystem {

    [BurstCompile]
    public struct MoveJob : IJobChunk {

        public float DeltaTime;
        public ArchetypeChunkComponentType<Translation> TranslationArchetypeChunk;
        public ArchetypeChunkComponentType<DirectionComponent> DirectionArchetypeChunk;
        [ReadOnly] public ArchetypeChunkComponentType<MoveSpeedComponent> MoveSpeedArchetypeChunk;
        [ReadOnly] public ArchetypeChunkComponentType<MoveLimitsComponent> MoveLimitsArchetypeChunk;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex) {
            NativeArray<Translation> translationArray = chunk.GetNativeArray(TranslationArchetypeChunk);
            NativeArray<DirectionComponent> directionArray = chunk.GetNativeArray(DirectionArchetypeChunk);
            NativeArray<MoveSpeedComponent> moveSpeedArray = chunk.GetNativeArray(MoveSpeedArchetypeChunk);
            NativeArray<MoveLimitsComponent> moveLimitsArray = chunk.GetNativeArray(MoveLimitsArchetypeChunk);

            for (int i = 0; i < chunk.Count; i++) {
                Translation translation = translationArray[i];
                DirectionComponent direction = directionArray[i];
                MoveSpeedComponent moveSpeed = moveSpeedArray[i];
                MoveLimitsComponent moveLimits = moveLimitsArray[i];

                translation.Value += direction.Value * DeltaTime * moveSpeed.Value;
                translationArray[i] = translation;

                if (translation.Value.x < moveLimits.Left || translation.Value.x > moveLimits.Right) {
                    direction.Value.x *= -1;
                    directionArray[i] = direction;
                }

                if (translation.Value.y < moveLimits.Bottom || translation.Value.y > moveLimits.Top) {
                    direction.Value.y *= -1;
                    directionArray[i] = direction;
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        EntityQuery notDeadEnemiesQuery = GetEntityQuery(
            typeof(Enemy),
            ComponentType.ReadWrite<Translation>(),
            ComponentType.ReadWrite<DirectionComponent>(),
            ComponentType.ReadOnly<MoveSpeedComponent>(),
            ComponentType.ReadOnly<MoveLimitsComponent>()
        );

        MoveJob moveJob = new MoveJob {
            DeltaTime = Time.DeltaTime,
            TranslationArchetypeChunk = GetArchetypeChunkComponentType<Translation>(false),
            DirectionArchetypeChunk = GetArchetypeChunkComponentType<DirectionComponent>(false),
            MoveSpeedArchetypeChunk = GetArchetypeChunkComponentType<MoveSpeedComponent>(true),
            MoveLimitsArchetypeChunk = GetArchetypeChunkComponentType<MoveLimitsComponent>(true)
        };

        return moveJob.Schedule(notDeadEnemiesQuery, inputDeps);
    }
}
*/

public class MoveSystem : ComponentSystem {

    protected override void OnUpdate() {
        Entities.ForEach((ref Translation translation, ref DirectionComponent direction, ref MoveSpeedComponent moveSpeed, ref MoveLimitsComponent moveLimits) => {
            translation.Value += direction.Value * Time.DeltaTime * moveSpeed.Value;

            if(translation.Value.x < moveLimits.Left || translation.Value.x > moveLimits.Right) {
                direction.Value.x *= -1;
            }

            if (translation.Value.y < moveLimits.Bottom || translation.Value.y > moveLimits.Top) {
                direction.Value.y *= -1;
            }
        });
    }
}

/*
public struct DeathTimerComponent : IComponentData {
    public float Value;
}

public class DeathTimerSystem : ComponentSystem {

    protected override void OnUpdate() {
        Entities.WithNone<DeathMark>().ForEach((Entity entity, ref DeathTimerComponent deathTimer) => {
            deathTimer.Value -= Time.DeltaTime;

            if(deathTimer.Value <= 0f) {
                PostUpdateCommands.AddComponent(entity, typeof(DeathMark));
            }
        });
    }
}

public struct DeathMark : IComponentData { }
*/