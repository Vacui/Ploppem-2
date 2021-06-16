using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GetGameStat : MonoBehaviour {

    private TMP_Text text;

    [SerializeField] GameStatsSystem.GameStat gameStat;
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        UpdateText(GameStatsSystem.GetStat(gameStat));
    }

    private void UpdateText(string value) {
        if (text == null) {
            return;
        }     

        text.text = prefix + value + suffix;
    }

}