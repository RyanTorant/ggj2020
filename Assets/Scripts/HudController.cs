using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject EnemyImage;
    public Text gameOverText;
    public GameObject pausePanel;
    public GameObject EnemyBar;

    public int enemiesCounter;
    public int startingEnemies;
    Vector3 firstPosition;
    Vector3 margin;
    public GameObject parent;
    
    List<GameObject> enemiesHudImage = new List<GameObject>();
    bool isPause = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2 offset = new Vector2(parent.GetComponent<RectTransform>().rect.width/3f, parent.GetComponent<RectTransform>().rect.height/2f);
        startingEnemies = enemiesCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        float xMin = parent.GetComponent<RectTransform>().position.x + parent.GetComponent<RectTransform>().rect.xMin - offset.x / 2.0f;
        float yMax = parent.GetComponent<RectTransform>().position.y + parent.GetComponent<RectTransform>().rect.yMin + offset.y;
        firstPosition = new Vector3(xMin, yMax, 0);
        Image enemySprite = EnemyImage.GetComponent<Image>();
        margin = new Vector3(enemySprite.rectTransform.sizeDelta.x * 3f, 0, 0);
        HudLoad();
        GameObject cam = GameObject.FindWithTag("HudCam");
        DestroyImmediate(cam);
        gameOverText.enabled = false;
        pausePanel.gameObject.SetActive(false);
        EnemyBar.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            pausePanel.gameObject.SetActive(isPause);
            Time.timeScale = isPause ? 0 : 1;
        }
    }

    public void KillEnemy()
    {
        //enemiesCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesCounter--;
        GameObject imageToDestroy = enemiesHudImage[enemiesHudImage.Count - 1];
        enemiesHudImage.RemoveAt(enemiesHudImage.Count - 1);
        GameObject.Destroy(imageToDestroy);
    }

    public void GameOver(bool youWon)
    {
        gameOverText.enabled = true;
        pausePanel.gameObject.SetActive(true);
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
            GameObject image = GameObject.Instantiate(EnemyImage, nextPos, Quaternion.identity,  parent.transform);
            nextPos += margin;
            enemiesHudImage.Add(image);
        }
    }
}
