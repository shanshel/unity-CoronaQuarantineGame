using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIBloodAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bloodObjects;
    void Start()
    {
        for (var x = 0; x < bloodObjects.Length; x++)
        {
            bloodObjects[x].transform.DOShakePosition(1f, 30f, 0, 40f, true).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutBounce) ;
            bloodObjects[x].transform.DOLocalMoveY(Random.Range(100f, 400f), Random.Range(1f, 2.5f)).SetLoops(-1, LoopType.Yoyo);
        }
    }


}
