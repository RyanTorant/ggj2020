using UnityEngine;
using System.Collections;

public enum SFX
{
    StartJump,
    EndJump,
    Grab,
    Release,
    Dead
};

public class AudioManager : MonoBehaviour
{
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
    private float musicVolume = 1;
    private float soundFXVolume = 1;

    public static void setVolume(bool isMusic, float volume)
    {
        if (instance != null)
        {
            if (isMusic)
            {
                instance.musicVolume = volume;
            }
            else
            {
                instance.soundFXVolume = volume;
            }
        }
    }

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

    public static void PlaySound(SFX sound)
    {
        if (instance != null)
        {
            instance.fxAudioSource.PlayOneShot(instance.soundsFX[(int)sound]);
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
            instance.StartCoroutine(FadeOut(instance.musicAudioSource1,fade));
        }
    }

    public static void PlayMoving(float fade)
    {
        if (instance != null)
        {
            instance.moveAudioSource.volume = 0;
            instance.moveAudioSource.Play();
            instance.StartCoroutine(FadeIn(instance.moveAudioSource, fade));
        }
    }

    public static void StopMoving(float fade)
    {
        if (instance != null)
        {
            instance.StartCoroutine(FadeOut(instance.moveAudioSource, fade));
        }
    }

    public static IEnumerator FadeAudio(float fadeTime)
    {
        if (fadeTime == 0.0f)
        {
            instance.musicAudioSource1.volume = instance.musicVolume;
            instance.musicAudioSource2.volume = 0.0f;
            instance.musicAudioSource2.Stop();
        }
        else
        {
            while (instance.musicAudioSource2.volume > 0)
            {
                float deltaVolume = instance.musicVolume * (Time.deltaTime / fadeTime);
                instance.musicAudioSource2.volume -= deltaVolume;
                instance.musicAudioSource1.volume += deltaVolume;

                yield return null;
            }

            instance.musicAudioSource1.volume = instance.musicVolume;
            instance.musicAudioSource2.volume = 0;
            instance.musicAudioSource2.Stop();
        }
    }

    public static IEnumerator FadeOut(AudioSource audio, float fadeTime)
    {
        if (fadeTime == 0.0f)
        {
            audio.volume = 0.0f;
            audio.Stop();
        }
        else
        {
            while (audio.volume > 0)
            {
                float deltaVolume = instance.musicVolume * (Time.deltaTime / fadeTime);
                 audio.volume -= deltaVolume;

                yield return null;
            }

            audio.volume = 0;
            audio.Stop();
        }
    }
    public static IEnumerator FadeIn(AudioSource audio, float fadeTime)
    {
        if (fadeTime == 0.0f)
        {
            audio.volume = instance.musicVolume;
        }
        else
        {
            while (audio.volume > 0)
            {
                float deltaVolume = instance.musicVolume * (Time.deltaTime / fadeTime);
                audio.volume += deltaVolume;

                yield return null;
            }
            audio.volume = instance.musicVolume;
        }
    }

    public static float GetMusicVol()
    {
        return instance.musicVolume;
    }

    public static float GetFXVol()
    {
        return instance.soundFXVolume;
    }
}