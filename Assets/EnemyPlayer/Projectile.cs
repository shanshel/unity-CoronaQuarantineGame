using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed;
    public float lifeTime;
    public GameObject explotion;
    void Start()
    {
        Invoke("destroyEFFECT", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="WALL1")
        {
            Destroy(gameObject);
        }
    }
    public void destroyEFFECT()
    {
        Instantiate(explotion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

