using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Utils;

[UpdateAfter(typeof(EnemyRenderingJobSystem))]
public class EnemyRenderingSystem : ComponentSystem {

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
                    lifetimeRenderingData.Layer, // Layer
                    Camera.main,
                    0, // Submesh index
                    materialPropertyBlock
                    );

            });

    }
}