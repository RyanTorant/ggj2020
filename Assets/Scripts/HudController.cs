using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject EnemyImage;
    public Text gameOverText;
    public Button restartButton;

    int enemiesCounter;
    Vector3 firstPosition;
    Vector3 margin;
    Canvas mainCanvas;
    Vector2 offset = new Vector2(20,-20);
    List<GameObject> enemiesHudImage = new List<GameObject>();
    bool isPause = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        enemiesCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        float xMin = mainCanvas.GetComponent<RectTransform>().position.x + mainCanvas.GetComponent<RectTransform>().rect.xMin + offset.x;
        float yMax = mainCanvas.GetComponent<RectTransform>().position.y + mainCanvas.GetComponent<RectTransform>().rect.yMax + offset.y;
        firstPosition = new Vector3(xMin, yMax, 0);
        Image enemySprite = EnemyImage.GetComponent<Image>();
        margin = new Vector3(enemySprite.rectTransform.sizeDelta.x * 1.5f, 0, 0);
        HudLoad();
        GameObject cam = GameObject.FindWithTag("HudCam");
        DestroyImmediate(cam);
        gameOverText.enabled = false;
        restartButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPause = !isPause;
            restartButton.gameObject.SetActive(isPause);
            Time.timeScale = isPause ? 0 : 1;
        }
    }

    public int KillEnemy()
    {
        enemiesCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        GameObject imageToDestroy = enemiesHudImage[enemiesHudImage.Count - 1];
        enemiesHudImage.RemoveAt(enemiesHudImage.Count - 1);
        GameObject.Destroy(imageToDestroy);
        return enemiesHudImage.Count;
    }

    public void GameOver(bool youWon)
    {
        gameOverText.enabled = true;
        restartButton.gameObject.SetActive(true);
        if (youWon)
        {
            gameOverText.text = "YOU WIN";
            gameOverText.color = Color.green;
        }
        else
        {
            gameOverText.text = "GAME OVER";
            gameOverText.color = Color.gray;
        }
    }

    void HudLoad()
    {
        Vector3 nextPos = firstPosition;
        for (int i=0; i < enemiesCounter; i++)
        {
            GameObject image = GameObject.Instantiate(EnemyImage, nextPos, Quaternion.identity,  mainCanvas.transform);
            nextPos += margin;
            enemiesHudImage.Add(image);
        }
    }
}
