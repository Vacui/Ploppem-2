using TMPro;
using UI;
using UnityEngine;

public class CreditPanel : MonoBehaviour {

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private TMP_Text authorText;
    [SerializeField] private OpenUrl openUrl;

    public void SetUp(Credit credit) {
        if (titleText != null) {
            titleText.text = credit.Title;
        }

        if (versionText != null) {
            versionText.text = credit.Version;
        }

        if (authorText != null) {
            authorText.text = credit.Author;
        }

        if(openUrl != null) {
            openUrl.Url = credit.Url;
        }
    }

}