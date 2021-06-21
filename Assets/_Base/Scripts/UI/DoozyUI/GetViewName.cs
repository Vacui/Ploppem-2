using Doozy.Engine.UI;
using System.Linq;
using TMPro;
using UnityEngine;

public class GetViewName : MonoBehaviour {

    [SerializeField] private TMP_Text text;
    [SerializeField] private string excludeViews;
    [SerializeField] private bool writerEffect;
    [SerializeField] [ShowIf(nameof(writerEffect), true)] private TextWriter textWriter;
    private string[] excludeViewsArray;

    private void Awake() {
        excludeViewsArray = excludeViews.Split(',');
    }

    public void GetName(UIView view) {
        if(text == null) {
            return;
        }

        string viewName = view.ViewName;

        if (excludeViewsArray.Contains(viewName)) {
            return;
        }

        if (writerEffect && textWriter != null) {
            textWriter.Write(text, viewName, .05f);
        } else {
            text.text = viewName;
        }
    }

    private void Update() {
        
    }

}