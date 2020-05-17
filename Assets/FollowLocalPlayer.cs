using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocalPlayer : MonoBehaviour
{
    public bool isFollow2D = true;

    // Update is called once per frame
    void Update()
    {
        if (NetworkPlayers._inst == null || NetworkPlayers._inst._localCPlayer == null) return;
        var pos = NetworkPlayers._inst._localCPlayer.transform.position;
        if (isFollow2D)
        {
            pos.z = transform.position.z;
        }
        transform.position = pos;
    }
}
