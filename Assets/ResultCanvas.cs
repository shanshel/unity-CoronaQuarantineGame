using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class ResultCanvas : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI doctorSlayerText, kingInfectorText, qurotineMaster, screenTitle;

    [SerializeField]
    Transform playersContainer;

    [SerializeField]
    UIPlayerResult UIPlayerPrefab;
    [SerializeField]
    GameObject winBack, loseBack, tieBack;


    private void Start()
    {
        setResult();
    }
    public void setResult()
    {
        var localPlayerStatistic = MatchStatistic.getLocalPlayerStatistic();
        var winnerTeam = MatchStatistic.getWinnerTeam();

        if (winnerTeam == EnumsData.Team.Both)
        {
            //Tie
            winBack.SetActive(false);
            loseBack.SetActive(false);
            tieBack.SetActive(true);
            screenTitle.text = "Tie";
        }
        else if (winnerTeam == localPlayerStatistic.playerTeam)
        {
            //You Won
            winBack.SetActive(true);
            loseBack.SetActive(false);
            tieBack.SetActive(false);
            screenTitle.text = "You Won";
        }
        else
        {
            winBack.SetActive(false);
            loseBack.SetActive(true);
            tieBack.SetActive(false);
            screenTitle.text = "You Lose";
        }


        doctorSlayerText.text = MatchStatistic.getDoctorSlayer().playerName.ToString();
        kingInfectorText.text = MatchStatistic.getBestInfector().playerName.ToString();
        qurotineMaster.text = MatchStatistic.getBestDoctor().playerName.ToString();
       




        foreach (var playerStats in MatchStatistic.getPlayerStatsOrderByKDA())
        {
            var uiPlayer = Instantiate(UIPlayerPrefab, playersContainer);
            uiPlayer.playerName.text = playerStats.playerName;
            uiPlayer.playerKills.text = playerStats.kills.ToString();
            uiPlayer.playerDeaths.text = playerStats.deaths.ToString();
            uiPlayer.playerBotKills.text = playerStats.botKills.ToString();

            if (playerStats.isMe)
            {
                uiPlayer.myPlayerBackground.SetActive(true);
            } else
            {
                uiPlayer.otherPlayerBackground.SetActive(false);
            }
        }
    }


    public void onLeave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.MainMenu);

    }

}
