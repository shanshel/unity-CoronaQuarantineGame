using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class AIBotController : MonoBehaviour
{
    [HideInInspector]
    public int botIndex;
    [HideInInspector]
    public bool isAlive = false;
    public Animator _animator;
    public GameObject visiablePart;
    public SpriteRenderer rendererHaead;
    CPlayer localPlayerCache;

    private void Start()
    {
        localPlayerCache = NetworkPlayers._inst._localCPlayer;
    }
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
 


    void renderToLocalTeam()
    {
      
        if (localPlayerCache._thisPlayerTeam == EnumsData.Team.Doctors)
        {

        }
        else
        {
            var hit = Physics2D.Raycast(transform.position, localPlayerCache._headSprite.transform.position - transform.position, 30f, LayerMask.GetMask("PatientPlayerLayer") | LayerMask.GetMask("Wall"));
            if (hit)
            {
                if (hit.transform.tag == "Patient")
                {
                    visiablePart.SetActive(true);
                }
                else
                {
                    visiablePart.SetActive(false);
                }
            }
        }
    }

}
