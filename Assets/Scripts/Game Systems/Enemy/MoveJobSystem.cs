using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class MoveJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref DirectionComponent direction, ref DirectionChangeTimerComponent changeDirectionTimer, in MoveSpeedComponent moveSpeed, in MoveLimitsComponent moveLimits) => {
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
    }
}