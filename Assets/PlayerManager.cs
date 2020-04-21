using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _inst;
    public GameObject mainPlayerObject;


    private void Awake()
    {
        _inst = this;
    }
}
