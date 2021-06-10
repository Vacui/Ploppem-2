using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(EnemyPreRenderingJobSystem))]
public class EnemyRenderingSystem : ComponentSystem {

    protected override void OnUpdate() {

        Material material = EnemySpawnerData.Instance.Material;
        Mesh mesh = EnemySpawnerData.Instance.Mesh;
        Gradient colorGradient = EnemySpawnerData.Instance.ColorGradient;

        Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref LifetimeComponent lifetime, ref EnemyRenderingData lifetimeRenderingData) => {

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

                materialPropertyBlock.SetColor("_Color", colorGradient.Evaluate(1f - lifetime.Value / lifetime.Start));

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