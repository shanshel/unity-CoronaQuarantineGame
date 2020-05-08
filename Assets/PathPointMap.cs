using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using Photon.Pun;

public class PathPointMap : MonoBehaviour
{
    public static PathPointMap _inst;
    public int botCount = 10;
    [SerializeField]
    private Transform[] points;

    [HideInInspector]
    public List<Transform> randomPoints = new List<Transform>();
    private void Start()
    {
       
        foreach (var arItem in points)
        {
            randomPoints.Add(arItem);
        }
        randomPoints.Shuffle();
        if (PhotonNetwork.IsMasterClient)
            InGameManager._inst.SpawnBots(randomPoints);
    }
    private void Awake()
    {
        _inst = this;
    }

 
}

public static class ListOpi
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
   
}
