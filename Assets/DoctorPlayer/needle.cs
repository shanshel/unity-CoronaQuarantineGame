using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class needle : MonoBehaviour
{
    public float needleSpeed;
    public float lifeTime;
    public GameObject explotion;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyEFFECT", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * needleSpeed * Time.deltaTime);
    }
    public void destroyEFFECT()
    {
        Instantiate(explotion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
