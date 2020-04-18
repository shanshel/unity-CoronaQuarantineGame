using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_SideWallColliderGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    BoxCollider2D _boxCollider;
    SpriteRenderer _renderer;
    Rigidbody2D _rigid;
    public bool isAlreadyShorten = false;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        buildCollider();
        Invoke("removeUnused", 4f);
    }

    void buildCollider()
    {

        _boxCollider.offset = new Vector2(0.03f, _renderer.size.y/2);
        _boxCollider.size = new Vector2(1.35f, _renderer.size.y);
        _boxCollider.enabled = true;
    }

    void removeUnused()
    {
        Destroy(_rigid);
        _boxCollider.isTrigger = false;
        Debug.Log("finished");
    }
}
