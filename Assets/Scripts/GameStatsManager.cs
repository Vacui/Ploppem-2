using System.IO;
using System.Xml.Serialization;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;

[UpdateBefore(typeof(ScoreSystem))]
public class GameStatsManager : MonoBehaviour {

    [System.Serializable]
    public class GameStat {
        public int Highscore;
        public int Games;
        public int Kills;
        public int Misses;
        public int Touches => Kills + Misses;
        public float Precision =>
            Touches > 0 ?
                ((float)Kills / Touches) * 100 :
                0f;

        public GameStat() {
            Highscore = 0;
            Games = 0;
            Kills = 0;
            Misses = 0;
        }
    }

    private static World world;

    private static GameStat globalGameStats;
    private static GameStat lastGameSessionStats;
    private static GameStat currentGameSessionStats;

    public enum GameStatSource {
        Global,
        LastGameSession,
        CurrentGameSession
    }

    public enum GameStatType {
        Highscore,
        Games,
        Kills,
        Misses,
        Touches,
        Precision
    }

    private const string GAME_STAT_FILENAME = "test.xml";
    private static string gameStatFilePath { get { return string.Format("{0}/{1}", Application.persistentDataPath, GAME_STAT_FILENAME); } }

    public static bool NewHighscore { get; private set; }
    public static event UnityAction OnNewHighscore;
    public static event UnityAction OnUpdateAllStats;

    public static void Initialize() {
        world = World.DefaultGameObjectInjectionWorld;

        world.GetOrCreateSystem<KillerEnemySystem>().OnMissedEnemy += MissedEnemy;
        world.GetOrCreateSystem<KillerEnemySystem>().OnKilledEnemy += KilledEnemy;

        DOTS_GameHandler.Instance.OnGameStarted += GameStarted;
        DOTS_GameHandler.Instance.OnGameOver += GameOver;

        ScoreSystem.OnScoreChanged += IsHighscore;

        OnUpdateAllStats += Save;
        OnUpdateAllStats += Load;

        Load();
    }

    private static void GameStarted() {
        NewHighscore = false;
        currentGameSessionStats = new GameStat();
        globalGameStats.Games++;
        Load();
    }

    private static void GameOver() {

        if (currentGameSessionStats == null) {
            Debug.LogWarning("No game session active");
            return;
        }

        if (globalGameStats == null) {
            Debug.LogWarning("No global stats to modify");
            return;
        }

        lastGameSessionStats = currentGameSessionStats;
        currentGameSessionStats = null;

        if (NewHighscore) {
            globalGameStats.Highscore = lastGameSessionStats.Highscore;
        }
        globalGameStats.Games += lastGameSessionStats.Games;
        globalGameStats.Kills += lastGameSessionStats.Kills;
        globalGameStats.Misses += lastGameSessionStats.Misses;

        Save();
    }

    private static void MissedEnemy() {
        if (currentGameSessionStats == null) {
            return;
        }

        currentGameSessionStats.Misses++;
    }

    private static void KilledEnemy(int enemies) {
        if (currentGameSessionStats == null) {
            return;
        }

        currentGameSessionStats.Kills += enemies;
    }

    private static void IsHighscore(int score) {
        if (currentGameSessionStats == null) {
            return;
        }

        if(score <= 0) {
            return;
        }

        currentGameSessionStats.Highscore = score;

        if (globalGameStats.Highscore < score) {
            globalGameStats.Highscore = score;
            if (!NewHighscore) {
                NewHighscore = true;
                OnNewHighscore?.Invoke();
            }
        }
    }


    public static string GetStat(GameStatSource source, GameStatType type) {

        GameStat stats = null;

        switch (source) {
            case GameStatSource.Global: stats = globalGameStats; break;
            case GameStatSource.LastGameSession: stats = lastGameSessionStats; break;
            case GameStatSource.CurrentGameSession: stats = currentGameSessionStats; break;
        }

        if (stats == null) {
            Debug.LogWarning("No game stats to read");
            return string.Empty;
        }

        switch (type) {
            case GameStatType.Highscore: return stats.Highscore.ToString();
            case GameStatType.Games: return stats.Games.ToString();
            case GameStatType.Kills: return stats.Kills.ToString();
            case GameStatType.Misses: return stats.Misses.ToString();
            case GameStatType.Touches: return stats.Touches.ToString();
            case GameStatType.Precision: return stats.Precision.ToString("#00.00");
        }

        return string.Empty;
    }

    private static void ForceLoad() {
        if (!File.Exists(gameStatFilePath)) {
            Clear();
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameStat));

        using (FileStream fs = new FileStream(gameStatFilePath, FileMode.Open)) {
            globalGameStats = (GameStat)serializer.Deserialize(fs);
        }

        Debug.Log("Game data loaded!");
    }

    private static void Load() {
        if(globalGameStats != null) {
            return;
        }

        ForceLoad();
    }

    private static void Save() {
        if (File.Exists(gameStatFilePath)) {
            File.Delete(gameStatFilePath);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameStat));

        using (StreamWriter writer = new StreamWriter(gameStatFilePath)) {
            serializer.Serialize(writer, globalGameStats);
        }

        Debug.Log("Game data saved!");
    }

    public static void Clear() {
        globalGameStats = new GameStat();
        Save();
        OnUpdateAllStats?.Invoke();
    }

}