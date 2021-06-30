using Unity.Jobs;
using Unity.Entities;
using UnityEngine.Events;

public class DOTS_GameHandler : JobComponentSystem {

    public static DOTS_GameHandler Instance { get; private set; }

    public event UnityAction OnGameStarted;
    public event UnityAction OnGamePaused;
    public event UnityAction OnGameResumed;
    public event UnityAction OnGameOver;

    protected override void OnCreate() {
        Instance = this;

        GameHandler.OnGameStarted += StartGame;
        GameHandler.OnGamePaused += PauseGame;
        GameHandler.OnGameResumed += ResumeGame;
        GameHandler.OnGameOver += GameOver;
    }

    protected override void OnStartRunning() {

        SetSystemsEnabled(false);

    }

    private void StartGame() {

        if (!SetSingletonValue(GameInfo.GameState.Playing, GameInfo.GameState.Playing, false)) {
            return;
        }

        SetSystemsEnabled(true);
        OnGameStarted?.Invoke();
    }

    private void PauseGame() {

        if (!SetSingletonValue(GameInfo.GameState.WaitingToStart, GameInfo.GameState.Playing)) {
            return;
        }

        SetSystemsEnabled(false);
        OnGamePaused?.Invoke();

    }

    private void ResumeGame() {

        if (!SetSingletonValue(GameInfo.GameState.Playing, GameInfo.GameState.WaitingToStart)) {
            return;
        }

        SetSystemsEnabled(true);
        OnGameResumed?.Invoke();

    }

    private void GameOver() {

        if (!SetSingletonValue(GameInfo.GameState.Dead, GameInfo.GameState.Dead, false)) {
            return;
        }

        SetSystemsEnabled(false);

        // Destroy all GameSession entities
        EntityQuery entityQuery = GetEntityQuery(typeof(Tag_GameSession));
        EntityManager.DestroyEntity(entityQuery);

        OnGameOver?.Invoke();

    }

    private bool SetSingletonValue(GameInfo.GameState value, GameInfo.GameState filter, bool filterEqual = true) {

        if (!HasSingleton<GameInfo>()) {
            return false;
        }

        GameInfo gameState = GetSingleton<GameInfo>();

        if ((filterEqual && gameState.State != filter) ||
            (!filterEqual && gameState.State == filter)) {
            return false;
        }

        gameState.State = value;
        SetSingleton(gameState);

        return true;

    }

    private void SetSystemsEnabled(bool enabled) {
        World.GetOrCreateSystem<EnemySpawnerSystem>().Enabled = enabled;
        World.GetOrCreateSystem<MoveJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<ChangeDirectionJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<ShowDirectionDebugJobSystem>().Enabled = enabled;
        World.GetOrCreateSystem<LifetimeJobSystem>().Enabled = enabled;
        //World.GetOrCreateSystem<EnemyPreRenderingJobSystem>().Enabled = enabled;
        //World.GetOrCreateSystem<EnemyRenderingSystem>().Enabled = enabled;
        World.GetOrCreateSystem<KillerEnemySystem>().Enabled = enabled;
        World.GetOrCreateSystem<ScoreSystem>().Enabled = enabled;
        World.GetOrCreateSystem<GameOverSystem>().Enabled = enabled;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        return default;
    }

}