using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class MoveJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        return Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref DirectionComponent direction, ref DirectionChangeTimerComponent changeDirectionTimer, in MoveSpeedComponent moveSpeed, in MoveLimitsComponent moveLimits) => {
                translation.Value += direction.Value * deltaTime * moveSpeed.Value;

                if (translation.Value.x < moveLimits.Left || translation.Value.x > moveLimits.Right) {
                    translation.Value.x = translation.Value.x < moveLimits.Left ? moveLimits.Left : (translation.Value.x > moveLimits.Right ? moveLimits.Right : translation.Value.x);
                    direction.Value.x *= -1;
                    changeDirectionTimer.Value = changeDirectionTimer.StartValue;
                }

                if (translation.Value.y < moveLimits.Bottom || translation.Value.y > moveLimits.Top) {
                    translation.Value.y = translation.Value.y < moveLimits.Bottom ? moveLimits.Bottom : (translation.Value.y > moveLimits.Top ? moveLimits.Top : translation.Value.y);
                    direction.Value.y *= -1;
                    changeDirectionTimer.Value = changeDirectionTimer.StartValue;
                }
            }).Schedule(inputDeps);
    }
}