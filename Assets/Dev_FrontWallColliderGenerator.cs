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

        Invoke("resizing", 2f);
        


    }

    void resizing()
    {
        _boxCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("bug bug");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (transform.position.y < collision.transform.position.y) return;
        Dev_SideWallColliderGenerator colScript = collision.GetComponent<Dev_SideWallColliderGenerator>();
        if (colScript != null)
        {
            if (colScript.isAlreadyShorten == false)
            {
                colScript.isAlreadyShorten = true;
                var pos = collision.ClosestPoint(transform.position);
                var otherBoxCollider = collision.GetComponent<BoxCollider2D>();
                var otherSpriteRenderer = collision.GetComponent<BoxCollider2D>();
                //var nextY = ( Mathf.Abs(pos.y) - otherSpriteRenderer.size.y ) + 5f ;
                
                otherBoxCollider.size = new Vector2(otherBoxCollider.size.x, otherBoxCollider.size.y - 1.5f);
                otherBoxCollider.offset = new Vector2(otherBoxCollider.offset.x, otherBoxCollider.size.y / 2);

            }
        

        }
  



    }

}
