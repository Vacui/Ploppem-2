public class StopButton : GameButton {

    protected override void Start() {
        GameHandler.OnGameStarted += EnableButton;
        GameHandler.OnGameOver += DisableButton;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= EnableButton;
        GameHandler.OnGameOver -= DisableButton;
        base.OnDestroy();
    }

    protected override void OnClick() {
        if (GameHandler.Instance == null) {
            return;
        }

        GameHandler.Instance.StopGame();
        base.OnClick();
    }

}