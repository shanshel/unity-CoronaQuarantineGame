using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAutoShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOShakePosition(3f, 15f, 0, 90f).SetLoops(-1, LoopType.Restart).SetEase(Ease.InBounce);
    }


}
