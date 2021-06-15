using System;
using Unity.Entities;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public static GameHandler Instance { get; private set; }

    public static event EventHandler OnGameStarted;
    public static event EventHandler OnGamePaused;
    public static event EventHandler OnGameResumed;
    public static event EventHandler OnGameOver;

    [EditorButton(nameof(ResumeGame), "Resume", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(PauseGame), "Pause", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StartGame), "Play", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StopGame), "Stop", ButtonActivityType.OnPlayMode)]
    [SerializeField] private int lifes;
    public int Lifes => lifes;

    private void Awake() {
        
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

    }

    private void Start() {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<GameOverSystem>().OnGameOver += (sender, args) => StopGame();
    }

    private void OnDestroy() {
        if (World.DefaultGameObjectInjectionWorld.IsCreated) {
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<GameOverSystem>().OnGameOver -= (sender, args) => StopGame();
        }
    }

    public void StartGame() {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public void PauseGame() {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame() {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    public void StopGame() {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

}