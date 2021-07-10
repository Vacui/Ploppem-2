using Unity.Entities;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private AudioClip killAudio;
    [SerializeField] private AudioClip gameOverAudio;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    private void Awake() {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<KillerEnemySystem>().OnKilledEnemy += KilledEnemy;
        GameHandler.OnGameOver += GameOver;
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<LifetimeJobSystem>().OnEnemyDead += EnemyDead;
    }

    private void EnemyDead() {
        PlaySound(deathAudio);
    }

    private void GameOver() {
        PlaySound(gameOverAudio);
    }

    private void KilledEnemy(int arg0) {
        PlaySound(killAudio);
    }

    private void PlaySound(AudioClip audio) {
        if(audio == null) {
            Debug.LogWarning("Audio is null");
            return;
        }

        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        if (audioMixerGroup != null) {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        audioSource.PlayOneShot(audio);
        Destroy(soundGameObject, audio.length);
    }
}