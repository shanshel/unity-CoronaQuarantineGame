using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Sticker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .25f).SetAutoKill(true).Play();
    }

   
}
