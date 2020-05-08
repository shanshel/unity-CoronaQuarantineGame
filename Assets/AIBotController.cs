using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBotController : MonoBehaviour
{
    [HideInInspector]
    public int botIndex;
    [HideInInspector]
    public bool isAlive = false;
    public Animator _animator;


    public void die()
    {
        if (isAlive == false) return;
        isAlive = false;
        _animator.SetTrigger("Dying");

    }
    public void revive()
    {
        if (isAlive == true) return;
        isAlive = true;
        _animator.SetTrigger("Reviving");
    }


    public void getInflected()
    {
        if (!isAlive)return;
        InGameManager._inst.UpdateBotStatus(botIndex, false);
    }

    
}
