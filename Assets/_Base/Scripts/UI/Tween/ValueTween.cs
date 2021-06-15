using UnityEngine;

public class ValueTween : TweenScript {

    [SerializeField] private float from;
    [SerializeField] private float to;
    private float value;

    [SerializeField] private UltEventFloat onUpdateCallback;

    protected override void ApplyTweenTypeSettings() {
        base.ApplyTweenTypeSettings();

        value = from;
        id = LeanTween.value(objectToAnimate, (value) => { onUpdateCallback?.Invoke(value); }, from, to, duration).id;
    }
}