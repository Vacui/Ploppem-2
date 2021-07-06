using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Score : MonoBehaviour {

    private TMP_Text text;

    private int tweenId = -1;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        ScoreSystem.OnScoreChanged += UpdateText;
        UpdateText(ScoreSystem.Score);
    }

    private void OnDestroy() {
        ScoreSystem.OnScoreChanged -= UpdateText;
    }

    private void UpdateText(int score) {
        if (text == null) {
            return;
        }

        text.text = score.ToString();

        if (tweenId > 0 && LeanTween.isTweening(tweenId)) {
            LeanTween.cancel(tweenId);
        }

        gameObject.transform.localScale = Vector3.one;
        tweenId = gameObject.LeanScale(Vector3.one * 1.5f, 0.3f).setLoopPingPong(1).id;
    }

}