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

    protected override void OnStartRunning() {
        SetSystemsEnabled(false);
    }

    private void StartGame(object sender, EventArgs args) {

        if (!SetSingletonValue(GameState.State.Playing, GameState.State.Playing, false)) {
            return;
        }

        SetSystemsEnabled(true);
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    private void PauseGame(object sender, EventArgs args) {

        if (!SetSingletonValue(GameState.State.WaitingToStart, GameState.State.Playing)) {
            return;
        }

        SetSystemsEnabled(false);
        OnGamePaused?.Invoke(this, EventArgs.Empty);

    }

    private void ResumeGame(object sender, EventArgs args) {

        if (!SetSingletonValue(GameState.State.Playing, GameState.State.WaitingToStart)) {
            return;
        }

        SetSystemsEnabled(true);
        OnGameResumed?.Invoke(this, EventArgs.Empty);

    }

    private void GameOver(object sender, EventArgs args) {

        if(!SetSingletonValue(GameState.State.Dead, GameState.State.Playing)) {
            return;
        }

        World.GetOrCreateSystem<EnemySpawnerJobSystem>().Reset();

        SetSystemsEnabled(false);

        // Destroy all Enemy entities
        EntityQuery entityQuery = GetEntityQuery(typeof(Enemy));
        EntityManager.DestroyEntity(entityQuery);

        OnGameOver?.Invoke(this, EventArgs.Empty);
        
    }

    private bool SetSingletonValue(GameState.State value, GameState.State filter, bool isFilter = true) {

        if (!HasSingleton<GameState>()) {
            return false;
        }

        GameState gameState = GetSingleton<GameState>();

        if ((isFilter && gameState.Value != filter) ||
            (!isFilter && gameState.Value == filter)) {
            return false;
        }

        gameState.Value = value;
        SetSingleton(gameState);

        return true;

    }

    private void SetSystemsEnabled(bool enabled) {
        World.GetOrCreateSystem<EnemySpawnerJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<MoveJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<ChangeDirectionJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<ShowDirectionDebugJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<LifetimeJobSystem>().Enabled = enabled;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        return default;
    }

}