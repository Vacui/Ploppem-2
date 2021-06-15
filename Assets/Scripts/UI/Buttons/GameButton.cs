using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameButton : MonoBehaviour {

    private Button button;

    protected virtual void Awake() {
        button = GetComponent<Button>();
    }

    protected virtual void Start() {
        if (button != null) {
            button.onClick.AddListener(OnClick);
        }
    }

    protected virtual void OnDestroy() {
        if (button != null) {
            button.onClick.RemoveListener(OnClick);
        }
    }

    protected virtual void OnClick() { }

    protected void DisableButton(object sender, EventArgs args) {
        if (button == null) {
            return;
        }

        button.interactable = false;
    }
    

    protected void EnableButton(object sender, EventArgs args) {
        if (button == null) {
            return;
        }

        button.interactable = true;
    }

}