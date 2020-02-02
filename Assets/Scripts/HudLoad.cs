using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudLoad : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        AudioManager.PlayMusic(Music.LevelMusic, 0.5f);
    }
}
