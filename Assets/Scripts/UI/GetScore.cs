using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GetScore : MonoBehaviour {

    private TMP_Text text;

    private int tweenId = -1;
    private const float SCALE_SIZE = 1.10f;
    private const float SCALE_TIME = 0.3f;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        if(text == null) {
            Debug.LogWarning("Text is null");
            return;
        }

        text.text = "";
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

        string scoreString = score.ToString();

        text.text = scoreString;

        if (tweenId > 0 && LeanTween.isTweening(tweenId)) {
            LeanTween.cancel(tweenId);
        }

        gameObject.transform.localScale = Vector3.one;
        tweenId = gameObject.LeanScale(Vector3.one * SCALE_SIZE, SCALE_TIME).setLoopPingPong(1).id;
    }

}