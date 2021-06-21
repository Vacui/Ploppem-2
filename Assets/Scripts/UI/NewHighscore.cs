using UnityEngine;

public class NewHighscore : MonoBehaviour {

    [SerializeField] private GameObject objectToShow;

    private void Start() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsSystem.OnNewHighscore += EnableObject;
        DisableObject();
    }

    private void OnDestroy() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsSystem.OnNewHighscore -= EnableObject;
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