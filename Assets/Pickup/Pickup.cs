using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public EnumsData.Team canBePickedBy;
    public BoxCollider2D _collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CPlayer player = collision.gameObject.GetComponent<CPlayer>();
        if (player != null)
        {
            _collider.enabled = false;
            if (player._thisPlayerTeam == canBePickedBy || canBePickedBy == EnumsData.Team.Both)
            {
                onPickedUp(player);
            }
        }
    }

    public virtual void onPickedUp(CPlayer palyerPickedIt)
    {

    }
}
