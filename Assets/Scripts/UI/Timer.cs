using System;
using TMPro;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Timer : MonoBehaviour {

    private World world;

    private TMP_Text text;

    private void OnEnable() {
        text = GetComponent<TMP_Text>();

        world = World.DefaultGameObjectInjectionWorld;
        world.GetOrCreateSystem<TimerSystem>().OnTimerChanged += UpdateTimerText;
        UpdateTimerText(world.GetOrCreateSystem<TimerSystem>().GameTime);
    }

    private void OnDisable() {
        if (world.IsCreated) {
            world.GetOrCreateSystem<TimerSystem>().OnTimerChanged -= UpdateTimerText;
        }
    }

    private void UpdateTimerText(float time) {
        if(text == null) {
            return;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string timeInText = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        if (timeSpan.Hours > 0) {
            timeInText += string.Format("{0:00}:" + timeInText, timeSpan.Hours);
        }

        text.text = timeInText;
    }
    private void UpdateTimerText(object sender, TimerSystem.TimerChangedEventArgs args) {
        UpdateTimerText(args.time);
    }

}