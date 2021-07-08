using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "UI/Skin", order = 2)]
public class Skin : ScriptableObject {

    public string Title => name;
    public int UnlockValue;
    public Sprite Icon;
    public Mesh Mesh;
}