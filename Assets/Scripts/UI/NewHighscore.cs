using UnityEngine;

public class NewHighscore : MonoBehaviour {

    [SerializeField] private GameObject objectToShow;

    private void Start() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsManager.OnNewHighscore += EnableObject;
        DisableObject();
    }

    private void OnDestroy() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsManager.OnNewHighscore -= EnableObject;
    }

    private void OnDisable() {
        DisableObject();
    }

    private void EnableObject() {
        if (objectToShow == null) {
            return;
        }

        objectToShow.SetActive(true);
    }
    private void DisableObject() {
        if (objectToShow == null) {
            return;
        }

        objectToShow.SetActive(false);
    }

}