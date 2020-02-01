using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDieTest : MonoBehaviour
{
    public HudController KillEnemy;

    private void OnDestroy()
    {
        KillEnemy.KillEnemy();
    }
}
