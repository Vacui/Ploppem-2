using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class GameStatsSystem : ComponentSystem {

    private World world;

    public enum GameStat {
        Highscore,
        Games,
        Misses,
        EnemiesKilled,
        Precision,
        Misses_GameSession,
        EnemiesKilled_GameSession,
        Precision_GameSession
    }

    // Global Stats
    private static readonly string HIGHSCORE_KEY = "highscore";
    private static float highscore = -1f;
    private static float Highscore {
        get { return highscore; }
        set {
            if (highscore > value) {
                return;
            }

            highscore = value;
            if (!NewHighscore) {
                NewHighscore = true;
                OnNewHighscore?.Invoke();
            }
        }
    }
    public static bool NewHighscore { get; private set; }
    public static event UnityAction OnNewHighscore;
    private static readonly string GAMES_KEY = "games";
    private static int Games { get; set; }

    private static readonly string MISSES_KEY = "misses";
    private static int Misses { get; set; }
    private static readonly string ENEMIES_KILLED_KEY = "enemies-killed";
    private static int Touches { get { return Misses + EnemiesKilled; } }
    private static int EnemiesKilled { get; set; }
    private static float Precision {
        get {
            return Touches > 0 ?
                ((float)EnemiesKilled / Touches) * 100 :
                0f;
        }
    }

    // Game Session stats
    private static int Misses_GameSession { get; set; }
    private static int EnemiesKilled_GameSession { get; set; }
    private static int Touches_GameSession { get { return Misses_GameSession + EnemiesKilled_GameSession; } }
    private static float Precision_GameSession {
        get {
            return Touches_GameSession > 0 ?
                ((float)EnemiesKilled_GameSession / Touches_GameSession) * 100 :
                0f;
        }
    }

    public static event UnityAction OnUpdateAllStats;


    protected override void OnCreate() {
        world = World.DefaultGameObjectInjectionWorld;

        TimerSystem.OnTimerChanged += OnHighscore;
        world.GetOrCreateSystem<KillerEnemySystem>().OnMissedEnemy += MissedEnemy;
        world.GetOrCreateSystem<KillerEnemySystem>().OnKilledEnemy += KilledEnemy;

        DOTS_GameHandler.Instance.OnGameStarted += GameStarted;
        DOTS_GameHandler.Instance.OnGameOver += SaveStats;

        OnUpdateAllStats += GetStats;

        GetStats();
    }

    protected override void OnDestroy() {
        TimerSystem.OnTimerChanged -= OnHighscore;

        DOTS_GameHandler.Instance.OnGameStarted -= GameStarted;
        DOTS_GameHandler.Instance.OnGameOver -= SaveStats;

        OnUpdateAllStats -= GetStats;

        SaveStats();
    }

    private void GameStarted() {
        NewHighscore = false;
        Misses_GameSession = 0;
        EnemiesKilled_GameSession = 0;
        Games++;
    }

    private void OnHighscore(float time) {
        Highscore = time;
    }

    private void MissedEnemy() {
        Misses_GameSession++;
        Misses++;
    }

    private void KilledEnemy(int enemies) {
        EnemiesKilled_GameSession += enemies;
        EnemiesKilled += enemies;
    }

    private void GetStats() {
        Highscore = PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f);
        Games = PlayerPrefs.GetInt(GAMES_KEY, 0);
        Misses = PlayerPrefs.GetInt(MISSES_KEY, 0);
        EnemiesKilled = PlayerPrefs.GetInt(ENEMIES_KILLED_KEY, 0);
    }
    public static string GetStat(GameStat stat) {
        switch (stat) {
            case GameStat.Highscore: return GetHighscoreFormatted();
            case GameStat.Games: return Games.ToString();
            case GameStat.Misses: return Misses.ToString();
            case GameStat.EnemiesKilled: return EnemiesKilled.ToString();
            case GameStat.Precision: return Precision.ToString("#.00");
            case GameStat.Misses_GameSession: return Misses_GameSession.ToString();
            case GameStat.EnemiesKilled_GameSession: return EnemiesKilled_GameSession.ToString();
            case GameStat.Precision_GameSession: return Precision_GameSession.ToString("#.00");
        }

        return string.Empty;
    }
    private static string GetHighscoreFormatted() {
        if (highscore >= 3600f) {
            return UtilsClass.FormatTimeWithHours(highscore);
        } else {
            return UtilsClass.FormatTime(highscore);
        }
    }

    private void SaveStats() {
        if (Highscore > PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f)) {
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, Highscore);
        }
        PlayerPrefs.SetInt(GAMES_KEY, Games);
        PlayerPrefs.SetInt(MISSES_KEY, Misses);
        PlayerPrefs.SetInt(ENEMIES_KILLED_KEY, EnemiesKilled);
    }

    public static void ClearStats() {
        PlayerPrefs.SetFloat(HIGHSCORE_KEY, 0f);
        PlayerPrefs.SetInt(GAMES_KEY, 0);
        PlayerPrefs.SetInt(MISSES_KEY, 0);
        PlayerPrefs.SetInt(ENEMIES_KILLED_KEY, 0);

        OnUpdateAllStats?.Invoke();
    }

    protected override void OnUpdate() { }
}