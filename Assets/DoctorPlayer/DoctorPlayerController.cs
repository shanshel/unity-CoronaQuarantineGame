using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 moveAmount;
    //animation
    private Animator anim;
    //needle
    public GameObject needle;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotTime;
    public GameObject weopon;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        AnimationFN();
        //
        needleFN();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
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
     
    }
    public void needleFN()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time >= shotTime)
            {
                Instantiate(needle, shotPoint.position, weopon.transform.rotation);
                shotTime = Time.time + timeBetweenShots;
            }
            
                   

        } 
    }


}
