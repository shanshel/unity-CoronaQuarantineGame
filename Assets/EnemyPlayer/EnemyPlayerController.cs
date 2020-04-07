using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 moveAmount;
    //sneeze
    public GameObject Sneeze;
    private float timer;
    //
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        timer += Time.deltaTime;
        //sneeze
        if (Input.GetKeyDown(KeyCode.C))
        {
            timer = 0;
            
            Invoke("destroyProjectile", 0.1f);
        }
        if(timer >= 10)
        {
            Invoke("destroyProjectile", 0.1f);
            timer = 0;
        }
        //
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }


     void destroyProjectile()
    {
        
        Instantiate(Sneeze, transform.position, Quaternion.identity);
        Destroy(Sneeze);
        
    }
}

