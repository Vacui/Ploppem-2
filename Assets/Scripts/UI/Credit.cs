using UnityEngine;

[CreateAssetMenu(fileName = "New Credit", menuName = "UI/Credit", order = 1)]
public class Credit : ScriptableObject {

    public string Title => name;
    public string Version;
    public string Author;
    public string Description;
    public string Url;

}