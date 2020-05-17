using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class PlayerStatistic
{
    public int playerKey;
    public string playerName;
    public int kills = 0, deaths = 0, botKills = 0;
    public Team playerTeam;
    public bool isMe;
}
