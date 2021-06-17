using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabConfirm : MonoBehaviour {

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void SetUp(string title, UnityAction yesCallback, UnityAction noCallback) {
        if(titleText != null) {
            titleText.text = title;
        }

        if (yesButton != null) {
            if (yesCallback != null) {
                yesButton.onClick.AddListener(yesCallback);
            }
            yesButton.onClick.AddListener(() => Destroy(gameObject));
        }

        if (noButton != null) {
            if (noCallback != null) {
                noButton.onClick.AddListener(noCallback);
            }
            noButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }

}