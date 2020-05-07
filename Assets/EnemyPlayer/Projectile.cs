using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D _rigid;
    public PolygonCollider2D _polyCollider;
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    public GameObject disapearingEffectPrefab;
    public Vector2 moveVector = Vector2.up;
    protected Vector2 lastVelocity;
    public PhotonView _photonView;
    void Start()
    {
        Invoke("disappear", lifeTime);
        _rigid.velocity = transform.up * bulletSpeed;
        Setup();
    }

    public virtual void Setup() { }


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
        }
        else if (collision.gameObject.layer == 12)
        {
            CPlayer player = collision.gameObject.GetComponent<CPlayer>();
            if (player.isInDevMode || player._photonView.IsMine) return;
            whenHitPlayer(collision);
        }
        else if (collision.gameObject.layer == 19)
        {
            whenHitBot(collision);
        }

    }
  

    public virtual void whenHitWall(Collision2D collision)
    {
        //_rigid.velocity = Vector3.zero;
        //bulletSpeed = 0;

        //moveVector = transform.localRotation.eulerAngles;
        //return;
        // Vector3 reflectedVelocity = n;

        //_rigid.velocity = reflectedVelocity;
        // rotate the object by the same ammount we changed its velocity
        //Quaternion rotation = Quaternion.FromToRotation(_rigid.velocity, reflectedVelocity);
        //transform.rotation = rotation * transform.rotation;
        //bulletSpeed = 0;
    }

    public virtual void whenHitBot(Collision2D collision)
    {
        _rigid.velocity = Vector3.zero;
        //bulletSpeed = 0;
    }

    public virtual void whenHitPlayer(Collision2D collision)
    {
        _rigid.velocity = Vector3.zero;
        //bulletSpeed = 0;
    }
    public virtual void disappear()
    {

        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        if (_photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

}

