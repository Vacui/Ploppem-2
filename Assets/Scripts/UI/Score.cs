﻿using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Score : MonoBehaviour {

    private TMP_Text text;

    private int tweenId = -1;
    private const float SCALE_SIZE = 1.25f;
    private const float SCALE_TIME = 0.3f;

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
        tweenId = gameObject.LeanScale(Vector3.one * SCALE_SIZE, SCALE_TIME).setLoopPingPong(1).id;
    }

}