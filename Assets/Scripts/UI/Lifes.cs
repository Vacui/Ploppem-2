using TMPro;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Lifes : MonoBehaviour {

    private World world;

    private TMP_Text text;

    private int tweenId = -1;

    private void OnEnable() {
        text = GetComponent<TMP_Text>();

        world = World.DefaultGameObjectInjectionWorld;
        world.GetOrCreateSystem<GameOverSystem>().OnLifesChanged += UpdateLifesText;
        UpdateLifesText(world.GetOrCreateSystem<GameOverSystem>().Lifes);
    }

    private void OnDisable() {
        if (world.IsCreated) {
            world.GetOrCreateSystem<GameOverSystem>().OnLifesChanged -= UpdateLifesText;
        }
    }
    
    private void UpdateLifesText(int remainingLifes) {
        if (text == null) {
            return;
        }

        text.text = remainingLifes.ToString();
    }
    private void UpdateLifesText(object sender, GameOverSystem.LifesEventArgs args) {
        UpdateLifesText(args.remainingLifes);

        if (tweenId > 0 && LeanTween.isTweening(tweenId)) {
            LeanTween.cancel(tweenId);
        }

        gameObject.transform.localScale = Vector3.one;
        tweenId = gameObject.LeanScale(Vector3.one * 1.15f, 0.3f).setLoopPingPong(1).id;
    }

}