using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject EnemyImage;
    int enemiesCounter;
    Vector3 firstPosition;
    Vector3 margin;
    Canvas mainCanvas;
    Vector2 offset = new Vector2(20,-20);
    List<GameObject> enemiesHudImage = new List<GameObject>();
    
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
    }

    public void KillEnemy()
    {
        enemiesCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        GameObject imageToDestroy = enemiesHudImage[enemiesHudImage.Count - 1];
        enemiesHudImage.RemoveAt(enemiesHudImage.Count - 1);
        GameObject.Destroy(imageToDestroy);
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
