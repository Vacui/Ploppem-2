using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Utils;

[UpdateBefore(typeof(MoveJobSystem))]
public class KillerEnemySystem : ComponentSystem {

    private const float SELECT_SIZE_RADIUS = .5f;

    protected override void OnUpdate() {

#if UNITY_EDITOR
        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        float3 clickPosition = UtilsClass.GetMouseWorldPosition();
        SelectEntitiesOn(clickPosition);
#endif

#if UNITY_ANDROID

        float3 touchPosition = UtilsClass.GetTouchWorldPosition(out bool valid);
        if (!valid) {
            return;
        }

        Debug.Log("Touch!");

        SelectEntitiesOn(touchPosition);
#endif
    }

    private void SelectEntitiesOn(float3 position) {

        float3 lowerLeftClickPosition = new float3(position.x - SELECT_SIZE_RADIUS, position.y - SELECT_SIZE_RADIUS, 0);
        float3 upperRightClickPosition = new float3(position.x + SELECT_SIZE_RADIUS, position.y + SELECT_SIZE_RADIUS, 0);

        int killedEnemies = 0;

        Entities
            .WithAll<Enemy>()
            .WithNone<DeathMark>()
            .ForEach((Entity entity, ref Translation translation, ref DeathAnimationData deathAnimData) => {

                float3 entityPosition = translation.Value;

                if (entityPosition.x < lowerLeftClickPosition.x ||
                    entityPosition.y < lowerLeftClickPosition.y ||
                    entityPosition.x > upperRightClickPosition.x ||
                    entityPosition.y > upperRightClickPosition.y) {
                    return;
                }

                killedEnemies++;

                // Mark the Entity as dead
                PostUpdateCommands.AddComponent(entity, typeof(DeathMark));
                deathAnimData.Killed = true;
            });

        if(killedEnemies <= 0) {
            return;
        }

        Debug.Log($"Killed {killedEnemies} enemies");

    }

}

public struct DeathMark : IComponentData { }