using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonsTweeing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ButtonAnimation()
    {
        transform.DOShakeScale(0.2f);
    }
}
