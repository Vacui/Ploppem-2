using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ShowDirectionDebugJobSystem : ComponentSystem {

    protected override void OnStartRunning() {
        Enabled = false;
    }

    protected override void OnUpdate() {
        Entities
            .WithAll<Tag_Enemy>()
            .WithNone<Tag_DeathMark>()
            .ForEach((ref Translation translation, ref MoveDirection direction) => {
                float drawDistance = 2f;
                Debug.DrawLine(translation.Value, translation.Value + (direction.Value * drawDistance));
            });
    }
}