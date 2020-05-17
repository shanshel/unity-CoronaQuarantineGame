using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsData;

public class MatchStatistic : MonoBehaviour
{
    public static Team winnerTeam = Team.Both;
    public static List<PlayerStatistic> playerStatstics = new List<PlayerStatistic>();

    public static void reset()
    {
        winnerTeam = Team.Both;
        playerStatstics.Clear();
        playerStatstics = new List<PlayerStatistic>();
    }
    public static void createPlayerStatistic(string name, Team team, int key)
    {
        PlayerStatistic ps = new PlayerStatistic();
        ps.playerKey = key;
        ps.playerName = name;
        ps.playerTeam = team;
        playerStatstics.Add(ps);
    }

    public static void updatePlayerStatistic(int key, int kills, int killsOnBot, int death) 
    {
        foreach (var ps in playerStatstics)
        {
            if (ps.playerKey == key)
            {
                ps.kills = kills;
                ps.botKills = killsOnBot;
                ps.deaths = death;
            }
        }
    }

    public static void calcWinner()
    {
        int doctorTeamPoints = 0, patientTeamPoints = 0;
        foreach (var ps in playerStatstics)
        {
            if (ps.playerTeam == Team.Doctors)
            {
                doctorTeamPoints += ps.kills + ps.botKills;
            }
            else
            {
                patientTeamPoints += ps.kills + ps.botKills;
            }
        }

        if (doctorTeamPoints > patientTeamPoints)
        {
            winnerTeam = Team.Doctors;
        }
        else if (patientTeamPoints > doctorTeamPoints)
        {
            winnerTeam = Team.Patients;
        }
        else
        {
            winnerTeam = Team.Both;
        }
    }

    public static void setWinner(Team team)
    {
        winnerTeam = team;
    }

    public static Team getWinnerTeam()
    {
        return winnerTeam;
    }

    public static PlayerStatistic getLocalPlayerStatistic()
    {
        for (var i = 0; i < playerStatstics.Count; i++)
        {
            if (playerStatstics[i].isMe)
            {
                return playerStatstics[i];
            }
        }
        return null;
    }

   
    public static PlayerStatistic getDoctorSlayer()
    {
        int indexToReturn = 0;
        int lastKillSave = 0;
        for (var i = 0; i < playerStatstics.Count; i++)
        {
            if (playerStatstics[i].playerTeam == Team.Patients)
            {
                if (lastKillSave < playerStatstics[i].kills)
                {
                    lastKillSave = playerStatstics[i].kills;
                    indexToReturn = i;
                }
            }
        }
        return playerStatstics[indexToReturn];
    }


    public static PlayerStatistic getBestInfector()
    {
        int indexToReturn = 0;
        int lastKillSave = 0;
        for (var i = 0; i < playerStatstics.Count; i++)
        {
            if (playerStatstics[i].playerTeam == Team.Patients)
            {
                if (lastKillSave < playerStatstics[i].botKills)
                {
                    lastKillSave = playerStatstics[i].botKills;
                    indexToReturn = i;
                }
            }
        }
        return playerStatstics[indexToReturn];
    }

    public static PlayerStatistic getBestDoctor()
    {
        int indexToReturn = 0;
        int lastKillSave = 0;
        for (var i = 0; i < playerStatstics.Count; i++)
        {
            if (playerStatstics[i].playerTeam == Team.Doctors)
            {
                if (lastKillSave < playerStatstics[i].kills)
                {
                    lastKillSave = playerStatstics[i].kills;
                    indexToReturn = i;
                }
            }
        }
        return playerStatstics[indexToReturn];
    }

    public static List<PlayerStatistic> getPlayerStatsOrderByKDA()
    {
        return playerStatstics.OrderBy(x => x.kills + x.deaths + x.botKills).ToList(); 

    }

}
