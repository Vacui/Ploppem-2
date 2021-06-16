using TMPro;
using Unity.Entities;
using UnityEngine;
using Utils;

[RequireComponent(typeof(TMP_Text))]
public class Timer : MonoBehaviour {

    private World world;

    private TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        world = World.DefaultGameObjectInjectionWorld;
        world.GetOrCreateSystem<TimerSystem>().OnTimerChanged += UpdateText;
        UpdateText(world.GetOrCreateSystem<TimerSystem>().GameTime);
    }

    private void OnDestroy() {
        if (world.IsCreated) {
            world.GetOrCreateSystem<TimerSystem>().OnTimerChanged -= UpdateText;
        }
    }

    private void UpdateText(float time) {
        if(text == null) {
            return;
        }

        if (time >= 3600f) {
            text.text = UtilsClass.FormatTimeWithHours(time);
        } else {
            text.text = UtilsClass.FormatTime(time);
        }
    }
    private void UpdateText(object sender, TimerSystem.TimerChangedEventArgs args) {
        UpdateText(args.time);
    }

}