using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    void Start()
    {


    }


    void Update()
    {


    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movedir = new Vector2(horizontal, vertical);
        rb.velocity = movedir * speed * Time.fixedDeltaTime;
    }

}
