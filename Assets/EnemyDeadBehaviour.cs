using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadBehaviour : StateMachineBehaviour
{
    public Color BadColor;
    public Color GoodColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Destroy(animator.gameObject);
        HudController HUD = GameObject.FindObjectOfType<HudController>();
        if (HUD != null)
        {
            int enemyCount = HUD.KillEnemy();

            // Change the background
            var cam = GameObject.Find("MainCamera").GetComponent<Camera>();
            cam.backgroundColor = Color.Lerp(BadColor, GoodColor, (float)enemyCount / (float)HUD.startingEnemies);

            // Check if you won
            if (enemyCount == 0)
            {
                HUD.GameOver(true);
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
