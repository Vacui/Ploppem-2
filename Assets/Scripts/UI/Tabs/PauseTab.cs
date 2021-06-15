public class PauseTab : GameTab {

    protected override void Start() {
        GameHandler.OnGameStarted += HideTab;
        GameHandler.OnGamePaused += ShowTab;
        GameHandler.OnGameResumed += HideTab;
        GameHandler.OnGameOver += HideTab;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= HideTab;
        GameHandler.OnGamePaused -= ShowTab;
        GameHandler.OnGameResumed -= HideTab;
        GameHandler.OnGameOver -= HideTab;
        base.OnDestroy();
    }

}