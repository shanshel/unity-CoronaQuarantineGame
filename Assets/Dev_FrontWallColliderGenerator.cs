using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_FrontWallColliderGenerator : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        buildCollider();
    }

    void buildCollider()
    {
    
        _boxCollider.offset = new Vector2(0 - (_renderer.size.x / 80f), .32f);
        _boxCollider.size = new Vector2(_renderer.size.x, .8f);
        _boxCollider.enabled = true;
  

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        if (transform.position.y < collision.gameObject.transform.position.y) return;
        Dev_SideWallColliderGenerator colScript = collision.gameObject.GetComponent<Dev_SideWallColliderGenerator>();
        if (colScript != null)
        {
            if (colScript.isAlreadyShorten == false)
            {
                colScript.isAlreadyShorten = true;
                var otherBoxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
                otherBoxCollider.size = new Vector2(otherBoxCollider.size.x, otherBoxCollider.size.y - 1.5f);
                otherBoxCollider.offset = new Vector2(otherBoxCollider.offset.x, otherBoxCollider.size.y / 2);

            }


        }
    }


 

}
