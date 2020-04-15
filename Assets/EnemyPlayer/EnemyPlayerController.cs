using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 moveAmount;
    public GameObject weopon;
    private bool isSneezing = true;
    private float timer;
    private float movingTIMER = 0;
    //animation
    private Animator anim;
    //bulet
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotTime;
   
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        timer += Time.deltaTime;
        //sneeze + bullet
        sneezeFN();
        
        //animation + movement
        AnimationFN();
        //weopne rotation
        weoponFN();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }
    public void sneezeFN()
        {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time >= shotTime)
            {
                Instantiate(projectile, shotPoint.position, weopon.transform.rotation);
                shotTime = Time.time + timeBetweenShots;
            }
            anim.SetTrigger("Sneez");
            timer = 0;
            isSneezing = false;


          //  Invoke("destroyProjectile", 0.1f);
        }
        if (timer >= 10)
        {
          //  Invoke("destroyProjectile", 0.1f);
            timer = 0;
        }
    }

    public void AnimationFN()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;
        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if(isSneezing == false)
        {
            moveAmount = Vector2.zero;
            
            movingTIMER += Time.deltaTime;
            if(movingTIMER >= 0.1)
            {
                isSneezing = true;
                movingTIMER = 0;
                
            }
        }
    }

    void weoponFN()
    {
        /*
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weopon.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle , Vector3.forward);
        weopon.transform.rotation = rotation;
        */
    }


    void destroyProjectile()
    {
        
       // Instantiate(Sneeze, transform.position, Quaternion.identity);
       // Destroy(Sneeze);
        
    }
}

