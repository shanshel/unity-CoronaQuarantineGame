using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Props")
        {
           // collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Props")
        {
            //collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
