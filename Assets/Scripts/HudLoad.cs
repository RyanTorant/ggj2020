using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudLoad : MonoBehaviour
{
    public int HUD;
    void Start()
    {
        SceneManager.LoadScene(HUD, LoadSceneMode.Additive);
    }
}
