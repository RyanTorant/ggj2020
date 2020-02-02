using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadBehaviour : StateMachineBehaviour
{
    public Color BadColor;
    public Color GoodColor;

    private Color FadeStartColor;
    private Color FadeEndColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HudController HUD = GameObject.FindObjectOfType<HudController>();
        if (HUD != null)
        {
            FadeStartColor = Color.Lerp(GoodColor, BadColor, (float)HUD.enemiesCounter / (float)HUD.startingEnemies);
            FadeEndColor = Color.Lerp(GoodColor, BadColor, (float)(HUD.enemiesCounter - 1) / (float)HUD.startingEnemies);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HudController HUD = GameObject.FindObjectOfType<HudController>();
        if (HUD != null)
        {
            Camera.main.backgroundColor = Color.Lerp(FadeStartColor, FadeEndColor, stateInfo.normalizedTime);
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Destroy(animator.gameObject);
        HudController HUD = GameObject.FindObjectOfType<HudController>();
        if (HUD != null)
        {
            HUD.KillEnemy();

            // Check if you won
            if (HUD.enemiesCounter == 0)
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
