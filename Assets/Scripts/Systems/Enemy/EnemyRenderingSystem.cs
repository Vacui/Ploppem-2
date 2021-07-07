using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Utils;

[UpdateAfter(typeof(EnemyPreRenderingJobSystem))]
public class EnemyRenderingSystem : ComponentSystem {

    protected override void OnUpdate() {

        if(EnemySpawnerData.Instance == null) {
            return;
        }

        Material material = EnemySpawnerData.Material;
        Mesh mesh = EnemySpawnerData.Instance.Mesh;
        Gradient colorGradient = EnemySpawnerData.Instance.ColorGradient;

        Entities
            .WithAll<Tag_Enemy>()
            .ForEach((ref Translation translation, ref Lifetime lifetime, ref RenderingData renderingData) => {

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

                //materialPropertyBlock.SetColor("_Color", colorGradient.Evaluate(1f - lifetime.Value / lifetime.Start));
                materialPropertyBlock.SetColor("_Color", renderingData.Color.ToColor());

                Graphics.DrawMesh(
                    mesh,
                    renderingData.Matrix,
                    material,
                    0, // Layer
                    Camera.main,
                    0, // Submesh index
                    materialPropertyBlock
                    );

            });

    }
}