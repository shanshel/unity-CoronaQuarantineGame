using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager._inst.stopAllMusic();
    }
}
