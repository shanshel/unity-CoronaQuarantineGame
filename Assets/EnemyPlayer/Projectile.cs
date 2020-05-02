using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D _rigid;
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    public GameObject disapearingEffectPrefab;
    public Vector2 moveVector = Vector2.up;
    void Start()
    {
        Invoke("disappear", lifeTime);
        _rigid.velocity = transform.up * bulletSpeed;

    }


    // Update is called once per frame

    void FixedUpdate()
    {
        //_rigid.MovePosition(_rigid.position + (moveVector * bulletSpeed * Time.fixedDeltaTime));
        //transform.Translate(moveVector * bulletSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            //whenHitWall(collision);
            /*
            Debug.Log("walllll shit");
            var oldVeloicty = _rigid.velocity;
            var contact = collision.contacts[0];
            var reflectedVelocity = Vector3.Reflect(_rigid.velocity, contact.normal);
            _rigid.isKinematic = true; 

            _rigid.velocity = reflectedVelocity;
            _rigid.isKinematic = false;
            //rotate the object by the same ammount we changed its velocity
            Quaternion rotation = Quaternion.FromToRotation(oldVeloicty, reflectedVelocity);
            transform.rotation = rotation * transform.rotation;
            */
        }

    }
    public void OnTriggerEnter2D(Collider2D collision)
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

    public virtual void whenHitWall(Collider2D collision)
    {
        _rigid.velocity = Vector3.zero;
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

    public virtual void whenHitBot(Collider2D collision)
    {
        _rigid.velocity = Vector3.zero;
        //bulletSpeed = 0;
    }

    public virtual void whenHitPlayer(Collider2D collision)
    {
        _rigid.velocity = Vector3.zero;
        //bulletSpeed = 0;
    }
    void disappear()
    {
        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

