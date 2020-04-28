using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class needleProjectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject needleDisappearEffect;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        Invoke("ShowDisappearEffect", 2.9f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        speed = 0;
    }
    void ShowDisappearEffect()
    {
        Instantiate(needleDisappearEffect, transform.position, Quaternion.identity);
    }
}
