using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorPlayerController : MonoBehaviour
{
    //prams
    public float speed;
    public GameObject needle;
    public Transform shotPoint;
    public float timeBetweenShots;
    public GameObject weopon;
    [SerializeField]
    GameObject aimObject;

    public GameObject doctorLight;
    public Quaternion lightAngle;
    private Rigidbody2D rb;
    private Vector2 moveAmount;
    private Animator anim;
    private float shotTime;

    public bool isMine;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
   
    }


    void Update()
    {
        AnimationFN();
        //needleFN();
        aimFN();
       
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }

    void aimFN()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimObject.transform.position = new Vector2(mousePos.x, mousePos.y);

        Vector2 dir = mousePos - doctorLight.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        lightAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        doctorLight.transform.rotation = rotation;


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

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= shotTime)
            {
                Instantiate(needle, shotPoint.position, weopon.transform.rotation);
                shotTime = Time.time + timeBetweenShots;
            }
        } 
    }




}
