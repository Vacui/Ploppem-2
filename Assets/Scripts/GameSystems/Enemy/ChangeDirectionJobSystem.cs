using Reese.Random;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(MoveJobSystem))]
public class ChangeDirectionJobSystem : JobComponentSystem {

    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        float deltaTime = Time.DeltaTime;

        NativeArray<Random> randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

        return Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .WithAll<Enemy>()
            .WithNone<DeathMark>()
            .ForEach((int nativeThreadIndex, ref DirectionComponent direction, ref DirectionChangeTimerComponent directionChangeTimer, in Translation translation) => {
                directionChangeTimer.Value -= deltaTime;

                if (directionChangeTimer.Value <= 0f) {
                    // Change direction
                    Random random = randomArray[nativeThreadIndex];

                    float angle = random.NextFloat(-180f, 180f) * math.PI * 2f;
                    float x = math.cos(angle);
                    float y = math.sin(angle);
                    direction.Value = new float3(x, y, 0);

                    randomArray[nativeThreadIndex] = random;

                    directionChangeTimer.Value = directionChangeTimer.StartValue;
                }
            }).Schedule(inputDeps);
    }
}