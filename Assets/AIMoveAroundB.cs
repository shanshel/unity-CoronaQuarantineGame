using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveAroundB : StateMachineBehaviour
{
    Vector2 moveTarget = Vector2.zero;
    List<PathPoint> usedPoints = new List<PathPoint>();
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
       var pointOverlap = Physics2D.OverlapCircle(animator.transform.position, 3f, 1 << (int)15);

        PathPoint point = pointOverlap.gameObject.GetComponent<PathPoint>();
        var targetPoint = point.getNewLinkedPoint(usedPoints);

        if (usedPoints.Contains(targetPoint))
        {
            usedPoints.Clear();
        }
        usedPoints.Add(targetPoint);
        moveTarget = targetPoint.transform.position;


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (moveTarget != Vector2.zero)
        {
            animator.transform.position = Vector3.MoveTowards(animator.transform.position, moveTarget, 10f * Time.deltaTime);

            if (Vector2.Distance(animator.transform.position, moveTarget) < .5f)
            {
                animator.SetBool("isMoveAround", false);
            }
        }
   
      
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
