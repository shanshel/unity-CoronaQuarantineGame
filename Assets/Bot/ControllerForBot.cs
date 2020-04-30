using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerForBot : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    private Rigidbody2D rb;
    public float colorChangingSpeed = 1f;
    public Color startColor;
    public Color endColor;
    float startTime;
    public Renderer rend1;
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        getinfected();
    }

    public void getinfected()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isInfected", true);
        float t = (Time.time - startTime) * colorChangingSpeed;
        rend1.material.color = Color.Lerp(startColor, endColor, t);
        }
    }
}
