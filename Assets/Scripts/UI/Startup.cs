using UnityEngine;

public class Startup : MonoBehaviour {

    [SerializeField] ShaderVariantCollection shaderVariantCollection;

    public void Execute() {
        if (!shaderVariantCollection.isWarmedUp) {
            shaderVariantCollection.WarmUp();
        }

        Application.targetFrameRate = 60;

    }

}