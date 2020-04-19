using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvLoader : MonoBehaviour
{
    public List<GameObject> props;
    public GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        LoadObjectClosetoPlayer();
    }
    void Start()
    {
        InvokeRepeating("LoadObjectClosetoPlayer", 1f, 1f);
    }

    void LoadObjectClosetoPlayer()
    {
      
        for (var x = 0; x < props.Count; x++)
        {
            if (Vector2.Distance(player.transform.position, props[x].transform.position) < 85f)
            {
                props[x].SetActive(true);
            }
            else
            {
                props[x].SetActive(false);
            }
           
        }
            /*
            if (Vector2.Distance( props[x].transform.position, player.transform.position) < 60f)
            {
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
                
                if (shadowCaster != null)
                {
                    shadowCaster.enabled = true;
                }

                if (collider != null)
                {
                    collider.enabled = true;
                }
              
            }
            else
            {
                if (renderer != null)
                {
                    renderer.enabled = false;
                }

                if (shadowCaster != null)
                {
                    shadowCaster.enabled = false;
                }

                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        
            */
        
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
