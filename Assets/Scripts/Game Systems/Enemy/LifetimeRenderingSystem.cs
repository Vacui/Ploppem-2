using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Utils;

[UpdateAfter(typeof(LifetimeRenderingJobSystem))]
public class LifetimeRenderingSystem : ComponentSystem {

    [BurstCompile]
    protected override void OnUpdate() {

        if (!HasSingleton<PrefabEntityComponent>()) {
            return;
        }

        Entity prefabEntity = GetSingleton<PrefabEntityComponent>().Prefab;
        RenderMesh renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(prefabEntity);

        Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref LifetimeComponent lifetime, ref LifetimeRenderingData lifetimeRenderingData) => {

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

                materialPropertyBlock.SetColor("_Color", lifetimeRenderingData.CurrentColor.ToColor());

                Graphics.DrawMesh(
                    renderMesh.mesh,
                    lifetimeRenderingData.Matrix,
                    renderMesh.material,
                    0, // Layer
                    Camera.main,
                    0, // Submesh index
                    materialPropertyBlock
                    );

            });

    }
}