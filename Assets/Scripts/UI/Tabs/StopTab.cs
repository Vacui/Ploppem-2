public class StopTab : GameTab {

    protected override void Start() {
        GameHandler.OnGameStarted += HideTab;
        GameHandler.OnGameOver += ShowTab;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= HideTab;
        GameHandler.OnGameOver -= ShowTab;
        base.OnDestroy();
    }

}