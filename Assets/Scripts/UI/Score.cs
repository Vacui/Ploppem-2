using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Score : MonoBehaviour {

    private TMP_Text text;

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
    }

}