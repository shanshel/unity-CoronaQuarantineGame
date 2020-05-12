using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D _rigid;
    public PolygonCollider2D _polyCollider;
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    public bool isLifetimeFinished;
    public int damage = 1;

    public GameObject disapearingEffectPrefab;
    public Vector2 moveVector = Vector2.up;
    protected Vector2 lastVelocity;
    public PhotonView _photonView;

    protected CPlayer bulletOwner;
    void Start()
    {
       
        _rigid.velocity = transform.up * bulletSpeed;

        bulletOwner = NetworkPlayers._inst.getCplayerByActorNumber(_photonView.CreatorActorNr);
        if (bulletOwner != null)
        {
            Debug.LogError("We Found the Owner For The Bullet");
        }
        else
        {
            Debug.LogError("There is No Owner to the bullet");
        }
        Setup();
    }

   
    public virtual void Setup() { }


    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (!isLifetimeFinished && lifeTime <= 0f)
        {
            isLifetimeFinished = true;
            disappear();
        }

        overwriteableUpdate();
    }

    public virtual void overwriteableUpdate()
    {

    }
    // Update is called once per frame


    void FixedUpdate()
    {
        lastVelocity = _rigid.velocity;
        //_rigid.MovePosition(_rigid.position + (moveVector * bulletSpeed * Time.fixedDeltaTime));
        //transform.Translate(moveVector * bulletSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            whenHitWall(collision);
            return;
        }

  
        if (collision.gameObject.tag == "Doctor")
        {
            whenHitDoctor(collision);
        }
        else if (collision.gameObject.tag == "Patient")
        {
            whenHitPatient(collision);
        }
        else if (collision.gameObject.tag == "BotAI")
        {
            whenHitBot(collision);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Doctor")
        {

            whenTriggerWithDoctor(collision);
        }
        else if (collision.gameObject.tag == "Patient")
        {

            whenTriggerWithPatient(collision);
        }
        else if (collision.gameObject.tag == "BotAI")
        {
            whenTriggerWithBot(collision);
        }
    }
    public virtual void whenHitWall(Collision2D collision)
    {
  
    }

    public virtual void whenHitDoctor(Collision2D collision)
    {
        _rigid.velocity = Vector3.zero;
    }

    public virtual void whenHitPatient(Collision2D collision)
    {
        _rigid.velocity = Vector3.zero;
    }
    public virtual void whenHitBot(Collision2D collision)
    {
        _rigid.velocity = Vector3.zero;
    }

    public virtual void whenTriggerWithDoctor(Collider2D collision)
    {

    }

    public virtual void whenTriggerWithPatient(Collider2D collision)
    {

    }
    public virtual void whenTriggerWithBot(Collider2D collision)
    {

    }

    public virtual void disappear()
    {

        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        if (_photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

}

