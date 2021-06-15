public class StartButton : GameButton {

    protected override void Start() {
        GameHandler.OnGameStarted += DisableButton;
        GameHandler.OnGameOver += EnableButton;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= DisableButton;
        GameHandler.OnGameOver -= EnableButton;
        base.OnDestroy();
    }

    protected override void OnClick() {
        if (GameHandler.Instance == null) {
            return;
        }

        GameHandler.Instance.StartGame();
        base.OnClick();
    }

}