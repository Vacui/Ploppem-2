public class StartTab : GameTab {

    protected override void Start() {
        GameHandler.OnGameStarted += ShowTab;
        GameHandler.OnGameOver += HideTab;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= ShowTab;
        GameHandler.OnGameOver -= HideTab;
        base.OnDestroy();
    }

}