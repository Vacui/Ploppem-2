using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Utils;

public class SelectEntitySystem : ComponentSystem {

    private const float SELECT_SIZE_RADIUS = .25f;

    protected override void OnUpdate() {

#if UNITY_EDITOR
        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        float3 clickPosition = UtilsClass.GetMouseWorldPosition();
        SelectEntitiesOn(clickPosition);
#endif

#if UNITY_ANDROID
        if (Input.touchCount <= 0) {
            return;
        }

        float3 touchPosition = UtilsClass.GetTouchWorldPosition(out bool valid);
        if (!valid) {
            return;
        }

        SelectEntitiesOn(touchPosition);
#endif
    }

    private void SelectEntitiesOn(float3 position) {

        float3 lowerLeftClickPosition = new float3(position.x - SELECT_SIZE_RADIUS, position.y - SELECT_SIZE_RADIUS, 0);
        float3 upperRightClickPosition = new float3(position.x + SELECT_SIZE_RADIUS, position.y + SELECT_SIZE_RADIUS, 0);

        Entities
            .WithNone<DeathMark>()
            .WithAll<Enemy>()
            .ForEach((Entity entity, ref Translation translation) => {

                float3 entityPosition = translation.Value;

                if (entityPosition.x < lowerLeftClickPosition.x ||
                    entityPosition.y < lowerLeftClickPosition.y ||
                    entityPosition.x > upperRightClickPosition.x ||
                    entityPosition.y > upperRightClickPosition.y) {
                    return;
                }

                PostUpdateCommands.AddComponent(entity, new DeathMark());
            });

    }

}

public struct DeathMark : IComponentData { }