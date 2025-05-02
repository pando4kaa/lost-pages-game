using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static AudioSource randomPitchAudioSource;
    private static AudioSource voiceAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();

            // Перевіряємо чи є хоча б один AudioSource
            if (audioSources.Length == 0)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                randomPitchAudioSource = gameObject.AddComponent<AudioSource>();
                voiceAudioSource = gameObject.AddComponent<AudioSource>();
            }
            // Якщо є один AudioSource, додаємо другий і третій
            else if (audioSources.Length == 1)
            {
                audioSource = audioSources[0];
                randomPitchAudioSource = gameObject.AddComponent<AudioSource>();
                voiceAudioSource = gameObject.AddComponent<AudioSource>();
            }
            // Якщо є два AudioSource, додаємо третій
            else if (audioSources.Length == 2)
            {
                audioSource = audioSources[0];
                randomPitchAudioSource = audioSources[1];
                voiceAudioSource = gameObject.AddComponent<AudioSource>();
            }
            // Якщо є три або більше AudioSource
            else
            {
                audioSource = audioSources[0];
                randomPitchAudioSource = audioSources[1];
                voiceAudioSource = audioSources[2];
            }

            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = Random.Range(1f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    public static void PlayVoice(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(clip);
    }

    // Start is called before the first frame update
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
