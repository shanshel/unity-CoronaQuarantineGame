using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvLoader : MonoBehaviour
{
    public Transform[] props;
    // Start is called before the first frame update

    void Start()
    {
        foreach (var prop in props)
        {
            prop.gameObject.SetActive(false);
        }

        InvokeRepeating("LoadObjectClosetoPlayer", 1f, 1f);
    }

    void LoadObjectClosetoPlayer()
    {
        if (NetworkPlayers._inst._localCPlayer == null)
            return;


        Debug.Log("i reach here");
        Debug.Log("distance from " + NetworkPlayers._inst._localCPlayer.transform.position);
        Debug.Log("distance To " + props[0].position);
        Debug.Log("IS: " + Vector2.Distance(NetworkPlayers._inst._localCPlayer.transform.position, props[0].position));
        for (var x = 0; x < props.Length; x++)
        {
            if (Vector2.Distance(NetworkPlayers._inst._localCPlayer.transform.position, props[x].position) < 60f)
            {
                props[x].gameObject.SetActive(true);
            }
            else
            {
                props[x].gameObject.SetActive(false);
            }
        }
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
