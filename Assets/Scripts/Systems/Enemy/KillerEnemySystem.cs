using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Utils;
using System;

[UpdateBefore(typeof(MoveJobSystem))]
public class KillerEnemySystem : ComponentSystem {

    private const float SELECT_SIZE_RADIUS = .5f;
    public event EventHandler OnMissedEnemy;
    public event EventHandler<KilledEnemyEventArgs> OnKilledEnemy;
    public class KilledEnemyEventArgs : EventArgs {
        public int Enemies;
    }

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

        SelectEntitiesOn(touchPosition);
#endif
    }

    private void SelectEntitiesOn(float3 position) {

        if (position.x < EnemySpawnerData.Instance.SpawnLimitLeft ||
            position.x > EnemySpawnerData.Instance.SpawnLimitRight ||
            position.y < EnemySpawnerData.Instance.SpawnLimitBottom ||
            position.y > EnemySpawnerData.Instance.SpawnLimitTop) {
            return;
        }

        float3 lowerLeftClickPosition = new float3(position.x - SELECT_SIZE_RADIUS, position.y - SELECT_SIZE_RADIUS, 0);
        float3 upperRightClickPosition = new float3(position.x + SELECT_SIZE_RADIUS, position.y + SELECT_SIZE_RADIUS, 0);

        int killedEnemies = 0;

        Entities
            .WithAll<Tag_Enemy>()
            .WithNone<Tag_DeathMark>()
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
                PostUpdateCommands.AddComponent(entity, typeof(Tag_DeathMark));
                deathAnimData.Killed = true;
            });

        if(killedEnemies <= 0) {
            OnMissedEnemy?.Invoke(this, EventArgs.Empty);
            return;
        }

        // Debug.Log($"Killed {killedEnemies} enemies");
        OnKilledEnemy?.Invoke(this, new KilledEnemyEventArgs { Enemies = killedEnemies });

    }

}