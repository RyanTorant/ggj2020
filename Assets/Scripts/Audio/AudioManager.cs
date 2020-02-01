using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static readonly int JumpSFX = 0;

    public static readonly int MainMenuMusic = 0;

    public AudioClip[] soundsFX;
    public AudioClip[] musics;

    public AudioClip characterMove;

    AudioSource moveAudioSource;

    AudioSource musicAudioSource1;
    AudioSource musicAudioSource2;

    AudioSource fxAudioSource;

    static AudioManager instance;

    private int audioSourcesNum = 4;
    private bool isMute;
    private float volume;

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length == audioSourcesNum)
        {
            moveAudioSource = audioSources[0];
            moveAudioSource.clip = characterMove;
            moveAudioSource.loop = true;

            musicAudioSource1 = audioSources[1];
            musicAudioSource2 = audioSources[2];
            musicAudioSource1.loop = true;
            musicAudioSource2.loop = true;

            fxAudioSource = audioSources[3];
        }
    }

    public static AudioManager Instance()
    {
        return instance;
    }

    public static void PlaySound(int sound)
    {
        if (instance != null)
        {
            instance.fxAudioSource.PlayOneShot(instance.soundsFX[sound]);
        }
    }

    public static void PlayMusic(int music, float fade = 0.0f)
    {
        if (instance != null)
        {
            AudioSource audioSourceSwap = instance.musicAudioSource1;
            instance.musicAudioSource1 = instance.musicAudioSource2;
            instance.musicAudioSource2 = audioSourceSwap;

            instance.musicAudioSource1.clip = instance.musics[music];

            instance.StartCoroutine(FadeAudio(fade));
        }
    }

    public static void StopMusic(float fade = 0.0f)
    {
        if (instance != null)
        {
            instance.StartCoroutine(FadeOut(fade));
        }
    }

    public static IEnumerator FadeAudio(float fadeTime)
    {
        if (fadeTime == 0.0f)
        {
            instance.musicAudioSource1.volume = instance.volume;
            instance.musicAudioSource2.volume = 0.0f;
            instance.musicAudioSource2.Stop();
        }
        else
        {
            while (instance.musicAudioSource2.volume > 0)
            {
                float deltaVolume = instance.volume * (Time.deltaTime / fadeTime);
                instance.musicAudioSource2.volume -= deltaVolume;
                instance.musicAudioSource1.volume += deltaVolume;

                yield return null;
            }

            instance.musicAudioSource1.volume = instance.volume;
            instance.musicAudioSource2.volume = 0;
            instance.musicAudioSource2.Stop();
        }
    }

    public static IEnumerator FadeOut(float fadeTime)
    {
        if (fadeTime == 0.0f)
        {
            instance.musicAudioSource1.volume = 0.0f;
            instance.musicAudioSource1.Stop();
        }
        else
        {
            while (instance.musicAudioSource1.volume > 0)
            {
                float deltaVolume = instance.volume * (Time.deltaTime / fadeTime);
                instance.musicAudioSource1.volume -= deltaVolume;

                yield return null;
            }

            instance.musicAudioSource1.volume = 0;
            instance.musicAudioSource1.Stop();
        }
    }


}