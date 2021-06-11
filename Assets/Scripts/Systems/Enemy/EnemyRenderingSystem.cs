using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Utils;

[UpdateAfter(typeof(EnemyPreRenderingJobSystem))]
public class EnemyRenderingSystem : ComponentSystem {

    protected override void OnUpdate() {

        Material material = EnemySpawnerData.Instance.Material;
        Mesh mesh = EnemySpawnerData.Instance.Mesh;
        Gradient colorGradient = EnemySpawnerData.Instance.ColorGradient;

        Entities
            .WithAll<Enemy>()
            .ForEach((ref Translation translation, ref LifetimeComponent lifetime, ref EnemyRenderingData renderingData) => {

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

                //materialPropertyBlock.SetColor("_Color", colorGradient.Evaluate(1f - lifetime.Value / lifetime.Start));
                materialPropertyBlock.SetColor("_Color", renderingData.Color.ToColor());

                Graphics.DrawMesh(
                    mesh,
                    renderingData.Matrix,
                    material,
                    renderingData.Layer, // Layer
                    Camera.main,
                    0, // Submesh index
                    materialPropertyBlock
                    );

            });

    }
}