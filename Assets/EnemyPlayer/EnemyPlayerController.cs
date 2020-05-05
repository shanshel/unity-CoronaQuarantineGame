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
    private Renderer _renderer;

    //bulet
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotTime;

    bool isMain;
    
    public SpriteRenderer[] allBodyPicesRenderer;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }




    void RenderOnlyIfSeenByDoctor()
    {
        /*
        if (_renderer.isVisible && PlayerManager._inst.mainPlayerObject != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerManager._inst.mainPlayerObject.transform.position - transform.position);
            Debug.DrawRay(transform.position, PlayerManager._inst.mainPlayerObject.transform.position - transform.position, Color.red);

            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Doctor")
            {
                var doctorScript = hit.transform.GetComponent<DoctorPlayerController>();
                var dir = (transform.position - PlayerManager._inst.mainPlayerObject.transform.position);
                var lightAngle = doctorScript.lightAngle.eulerAngles.z;

                Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
                float doctorToEnemyAngle = rotation.eulerAngles.z;
    

                if (Mathf.DeltaAngle(lightAngle, doctorToEnemyAngle) < 60f && Mathf.DeltaAngle(lightAngle, doctorToEnemyAngle) > -60f)
                {
                    for (var x = 0; x < allBodyPicesRenderer.Length; x++)
                    {
                        allBodyPicesRenderer[x].enabled = true;
                        hit.transform.GetComponent<DoctorPlayerController>().onSeeEnemy();
                    }
                }
                else
                {
                    for (var x = 0; x < allBodyPicesRenderer.Length; x++)
                    {
                        allBodyPicesRenderer[x].enabled = false;
                    }
                }

           
            }
            else
            {
                for (var x = 0; x < allBodyPicesRenderer.Length; x++)
                {
                    allBodyPicesRenderer[x].enabled = false;
                }
            }
        }
        else
        {
            for (var x = 0; x < allBodyPicesRenderer.Length; x++)
            {
                allBodyPicesRenderer[x].enabled = false;
            }
        }
        */

    }
    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
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


          
        }
        if (timer >= 10)
        {
          
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


   
}

