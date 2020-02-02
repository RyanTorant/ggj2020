using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider musicVol;
    public Slider fxVol;

    private void Start()
    {
        musicVol.value = AudioManager.GetMusicVol();
        fxVol.value = AudioManager.GetFXVol();
    }

    // Start is called before the first frame update
    public void ChangeMusicVolume(float volume)
    {
        AudioManager.setVolume(true, volume);
    }

    public void ChangeFXVolume(float volume)
    {
        AudioManager.setVolume(false, volume);
    }
}
