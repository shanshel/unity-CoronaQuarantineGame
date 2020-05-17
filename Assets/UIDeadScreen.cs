using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIDeadScreen : UIOverlay
{
    [SerializeField]
    TextMeshProUGUI timeToRespawnText;

    private void Start()
    {
        InvokeRepeating("UpdateTimer", 0f, 1f);
    }
    void Update()
    {
        
    }

    void UpdateTimer()
    {
        if (NetworkPlayers._inst == null || NetworkPlayers._inst._localCPlayer == null)
        {
            return;
        }
        var timer = NetworkPlayers._inst._localCPlayer.respawnTimer;
        if (timer < 0) timer = 0;
        timeToRespawnText.text = Mathf.FloorToInt(timer).ToString() + " Sec";
    }

    public void specteteNext()
    {
        InGameManager._inst.setDeadFollowNexT();
    }
}
