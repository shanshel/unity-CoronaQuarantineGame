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
    public Animator doctorAnimator;
    public ParticleSystem dirt;
    float timeCount = 1f;
    private float surpisedTimer = 0f;
    public bool isMine;

    public PlayerCharacterNetwork _charInNetwork;
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!_charInNetwork._photonView.IsMine) return;
        surpisedTimer -= Time.deltaTime;
        AnimationFN();
        //needleFN();
        aimFN();
        timeCount -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_charInNetwork._photonView.IsMine) return;
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

            doctorAnimator.SetBool("moved", true);
            if (timeCount <= 0f)
            {
                Instantiate(dirt, transform.position, Quaternion.identity);
                timeCount = 1f;
            }
            
        }
        else
        {
            doctorAnimator.SetBool("moved", false);
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

    public void onSeeEnemy()
    {
        if (surpisedTimer <= 0)
        {
            doctorAnimator.SetTrigger("Surprised");
            surpisedTimer = 10f;
        }
        else
        {
            
        }
        

    }




}
