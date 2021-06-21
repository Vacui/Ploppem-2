using TMPro;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Lifes : MonoBehaviour {

    private World world;

    private TMP_Text text;

    private int tweenId = -1;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        world = World.DefaultGameObjectInjectionWorld;
        world.GetOrCreateSystem<GameOverSystem>().OnLifesChanged += UpdateText;
        UpdateText(world.GetOrCreateSystem<GameOverSystem>().Lifes);
    }

    private void OnDestroy() {
        if (world.IsCreated) {
            world.GetOrCreateSystem<GameOverSystem>().OnLifesChanged -= UpdateText;
        }
    }

    private void UpdateText(int remainingLifes) {
        if (text == null) {
            return;
        }

        if(remainingLifes <= 0) {
            text.text = "";
            return;
        }

        text.text = remainingLifes.ToString();

        if (tweenId > 0 && LeanTween.isTweening(tweenId)) {
            LeanTween.cancel(tweenId);
        }

        gameObject.transform.localScale = Vector3.one;
        tweenId = gameObject.LeanScale(Vector3.one * 1.15f, 0.3f).setLoopPingPong(1).id;
    }

}