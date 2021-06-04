using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Utils;

[UpdateAfter(typeof(LifetimeRenderingJobSystem))]
public class LifetimeRenderingSystem : ComponentSystem {

    protected override void OnUpdate() {

        Material material = EnemySpawnData.Instance.Material;
        Mesh mesh = EnemySpawnData.Instance.Mesh;

        Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref LifetimeComponent lifetime, ref LifetimeRenderingData lifetimeRenderingData) => {

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

                materialPropertyBlock.SetColor("_Color", lifetimeRenderingData.CurrentColor.ToColor());

                Graphics.DrawMesh(
                    mesh,
                    lifetimeRenderingData.Matrix,
                    material,
                    0, // Layer
                    Camera.main,
                    0, // Submesh index
                    materialPropertyBlock
                    );

            });

    }
}