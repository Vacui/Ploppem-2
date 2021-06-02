using System;
using Unity.Jobs;
using Unity.Entities;

public class DOTS_GameHandler : JobComponentSystem {

    public static DOTS_GameHandler Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    public event EventHandler OnGameOver;

    protected override void OnCreate() {
        Instance = this;

        Testing.OnGameStarted += StartGame;
        Testing.OnGamePaused += PauseGame;
        Testing.OnGameResumed += ResumeGame;
        Testing.OnGameOver += GameOver;
    }

    protected override void OnDestroy() {
        Testing.OnGameStarted -= StartGame;
        Testing.OnGamePaused -= PauseGame;
        Testing.OnGameResumed -= ResumeGame;
        Testing.OnGameOver -= GameOver;
    }

    private void StartGame(object sender, EventArgs args) {
        if (HasSingleton<GameState>()) {
            GameState gameState = GetSingleton<GameState>();
            if (gameState.Value != GameState.State.Playing) {
                gameState.Value = GameState.State.Playing;
                SetSingleton(gameState);

                SetSystemsEnabled(true);
                OnGameStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void PauseGame(object sender, EventArgs args) {
        if (HasSingleton<GameState>()) {
            GameState gameState = GetSingleton<GameState>();
            if (gameState.Value == GameState.State.Playing) {
                gameState.Value = GameState.State.WaitingToStart;
                SetSingleton(gameState);

                SetSystemsEnabled(false);
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void ResumeGame(object sender, EventArgs args) {
        if (HasSingleton<GameState>()) {
            GameState gameState = GetSingleton<GameState>();
            if (gameState.Value == GameState.State.WaitingToStart) {
                gameState.Value = GameState.State.Playing;
                SetSingleton(gameState);

                SetSystemsEnabled(true);
                OnGameResumed?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameOver(object sender, EventArgs args) {
        if (HasSingleton<GameState>()) {
            GameState gameState = GetSingleton<GameState>();
            gameState.Value = GameState.State.Dead;
            SetSingleton(gameState);

            SetSystemsEnabled(false);

            // Destroy all NOT GameState entities
            EntityQueryDesc entityQueryDesc = new EntityQueryDesc {
                None = new ComponentType[] { typeof(GameState) }
            };
            EntityQuery entityQuery = GetEntityQuery(entityQueryDesc);
            EntityManager.DestroyEntity(entityQuery);

            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetSystemsEnabled(bool enabled) {
        
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        return default;
    }

}