using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundVolumeController : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite[] iconArray;

    private const string SOUND_VOLUME_KEY = "volume";
    private float volume {
        get { return Mathf.Clamp(PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 0), 0f, 1f); }
        set {
            audioMixer.SetFloat("Volume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, Mathf.Clamp(value, 0f, 1f)); }
    }

    private void OnEnable() {
        volume = volume;
        UpdateVisual(
            Mathf.FloorToInt(GetVolumeIndex()),
            volume);
    }

    private float GetVolumeIndex() {
        return Utils.UtilsClass.Map(volume, 0, 1, 0, iconArray.Length - 1);
    }

    private void UpdateVolume() {
        float volumeIndex = GetVolumeIndex();
        volumeIndex++;
        if (volumeIndex >= iconArray.Length) {
            volumeIndex = 0;
        }
        Debug.Log(volumeIndex);
        float volumeIndexBaseZero = Utils.UtilsClass.Map(volumeIndex, 0, iconArray.Length - 1, 0, 1);
        volume = volumeIndexBaseZero;

        UpdateVisual(Mathf.FloorToInt(volumeIndex), volumeIndexBaseZero);

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.mute = volumeIndex <= 0;
            GetComponent<AudioSource>().Play();
        } else {
            Debug.LogWarning("Audio Source is null");
        }
    }

    private void UpdateVisual(int iconIndex, float sliderValue) {
        if (iconImage != null) {
            iconIndex = Mathf.Clamp(iconIndex, 0, iconArray.Length - 1);
            iconImage.sprite = iconArray[iconIndex];
        } else {
            Debug.LogWarning("Image is null");
        }

        if (slider != null) {
            slider.value = sliderValue;
        } else {
            Debug.LogWarning("Slider is null");
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        UpdateVolume();
    }
}