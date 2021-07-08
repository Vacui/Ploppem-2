using UnityEngine;

public class NewSkin : MonoBehaviour {

    [SerializeField] private GameObject objectToToggle;

    private void OnEnable() {
        if(objectToToggle == null) {
            Debug.Log("Object to toggle is null");
            return;
        }

        objectToToggle.SetActive(SkinManager.NewSkins);
    }

}