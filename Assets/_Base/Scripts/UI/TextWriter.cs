// based on CodeMonkey tutorial
// source: https://www.youtube.com/watch?v=ZVh4nH8Mayg

using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour {

    private TMP_Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    [SerializeField] private bool invisibleCharacters;

    public void Write(TMP_Text uiText, string textToWrite, float timePerCharacter) {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
        timer = 0;
    }

    private void Update() {
        if(uiText == null) {
            return;
        }

        if(textToWrite == "") {
            return;
        }

        timer -= Time.deltaTime;

        while(timer <= 0f) {
            timer += timePerCharacter;
            characterIndex++;
            string text = textToWrite.Substring(0, characterIndex);
            if (invisibleCharacters) {
                text += string.Format("<color=#00000000>{0}</color>", textToWrite.Substring(characterIndex));
            }

            uiText.text = text;

            if (characterIndex >= textToWrite.Length) {
                textToWrite = "";
                return;
            }
        }
    }

}