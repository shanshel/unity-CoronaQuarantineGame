using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvLoader : MonoBehaviour
{
    public List<GameObject> props;
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
            if (Vector2.Distance(PlayerManager._inst.mainPlayerObject.transform.position, props[x].transform.position) < 60f)
            {
                props[x].SetActive(true);
            }
            else
            {
                props[x].SetActive(false);
            }
           
        }
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
