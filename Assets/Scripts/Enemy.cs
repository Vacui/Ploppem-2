﻿using Reese.Random;
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

public struct DirectionChangeTimerComponent : IComponentData {
    public float StartValue;
    public float Value;
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

public class MoveJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref DirectionComponent direction, ref DirectionChangeTimerComponent changeDirectionTimer, in MoveSpeedComponent moveSpeed, in MoveLimitsComponent moveLimits) => {
            translation.Value += direction.Value * deltaTime * moveSpeed.Value;

            if (translation.Value.x < moveLimits.Left || translation.Value.x > moveLimits.Right) {
                translation.Value.x = Mathf.Clamp(translation.Value.x, moveLimits.Left, moveLimits.Right);
                direction.Value.x *= -1;
                changeDirectionTimer.Value = changeDirectionTimer.StartValue;
            }

            if (translation.Value.y < moveLimits.Bottom || translation.Value.y > moveLimits.Top) {
                translation.Value.y = Mathf.Clamp(translation.Value.y, moveLimits.Bottom, moveLimits.Top);
                direction.Value.y *= -1;
                changeDirectionTimer.Value = changeDirectionTimer.StartValue;
            }
        }).Schedule(inputDeps);

        return jobHandle;
    }
}

[UpdateAfter(typeof(MoveJobSystem))]
public class ChangeDirectionJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        NativeArray<float3> moveDirectionsArray = new NativeArray<float3>(8, Allocator.TempJob);
        moveDirectionsArray[0] = new float3(0, 1f, 0); // Up
        moveDirectionsArray[1] = new float3(0.5f, 0.5f, 0); // Up-Right
        moveDirectionsArray[2] = new float3(1f, 0, 0); // Right
        moveDirectionsArray[3] = new float3(0.5f, -0.5f, 0); // Down-Right
        moveDirectionsArray[4] = new float3(0, -1f, 0); // Down
        moveDirectionsArray[5] = new float3(-0.5f, -0.5f, 0); // Down-Left
        moveDirectionsArray[6] = new float3(-1f, 0, 0); // Left
        moveDirectionsArray[7] = new float3(-0.5f, 0.5f, 0); // Up-Left

        NativeArray<Unity.Mathematics.Random> randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

        JobHandle jobHandle = Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .ForEach((int nativeThreadIndex, ref DirectionComponent direction, ref DirectionChangeTimerComponent directionChangeTimer, in Translation translation) => {
                directionChangeTimer.Value -= deltaTime;

                if (directionChangeTimer.Value <= 0f) {
                    // Change direction
                    Unity.Mathematics.Random random = randomArray[nativeThreadIndex];

                    float angle = random.NextFloat(-180f, 180f) * Mathf.PI * 2f;
                    float x = Mathf.Cos(angle);
                    float y = Mathf.Sin(angle);
                    direction.Value = new float3(x, y, 0);

                    randomArray[nativeThreadIndex] = random;

                    directionChangeTimer.Value = directionChangeTimer.StartValue;
                }
            }).Schedule(inputDeps);

        jobHandle.Complete();

        moveDirectionsArray.Dispose();

        return inputDeps;
    }
}

public class ShowDirectionDebugJobSystem : ComponentSystem {

    protected override void OnUpdate() {
        Entities.ForEach((ref Translation translation, ref DirectionComponent direction) => {
            float drawDistance = 2f;
            Debug.DrawLine(translation.Value, translation.Value + (direction.Value * drawDistance));
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