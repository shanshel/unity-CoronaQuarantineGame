using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowCasterEditor : MonoBehaviour
{
    public ShadowCaster2D shadowCaster;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private void Awake()
    {

    }
    void Start()
    {

        Invoke("doSomething", 2f);


    }

    void doSomething()
    {
      

    }


}
