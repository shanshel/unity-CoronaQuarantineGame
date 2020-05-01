using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniMap : MonoBehaviour
{
    public SpriteRenderer _sprite;
    // Start is called before the first frame update


    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void setColor(Color color)
    {
        _sprite.color = color;
    }

}
