using UnityEngine;
using UnityEngine.EventSystems;

public class OpenUrl : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private string url;
    public string Url {
        get { return url; }
        set { url = value; }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Open();
    }

    public void Open() {
        if(url == "") {
            return;
        }

        Application.OpenURL(url);
    }

}