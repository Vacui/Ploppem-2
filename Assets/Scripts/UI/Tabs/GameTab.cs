using System;
using UnityEngine;
using UI;

[RequireComponent(typeof(Tab))]
public class GameTab : MonoBehaviour {

    private Tab tab;

    protected virtual void Awake() {
        tab = GetComponent<Tab>();
    }

    protected virtual void Start() { }

    protected virtual void OnDestroy() { }

    protected virtual void ShowTab(object sender, EventArgs args) {
        if (tab == null) {
            return;
        }

        tab.Active();
    }

    protected virtual void HideTab(object sender, EventArgs args) {
        if (tab == null) {
            return;
        }

        tab.Inactive();
    }

}