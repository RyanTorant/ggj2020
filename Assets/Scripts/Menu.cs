using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    private void Start()
    {
        AudioManager.PlayMusic(Music.MainMenudMusic, 0.0f);
    }

    public void SelectedButton(string button)
    {
        SceneManager.LoadScene(button);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
