using Doozy.Engine;
using Unity.Entities;
using UnityEngine;

public class UIHandler : MonoBehaviour {

    private delegate void GameOver();
    private GameOver gameOverDelegate;

    [SerializeField] private string gameOverGameEvent;

    private void Awake() {
        LeanTween.init(10);
    }

    private void Start() {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<GameOverSystem>().OnGameOver += () => {
            GameEventMessage.SendEvent(gameOverGameEvent);
        };
    }

}