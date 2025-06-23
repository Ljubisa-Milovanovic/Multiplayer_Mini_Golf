using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoardManager : NetworkBehaviour
{
    public TextMeshProUGUI[] rows;

    public static ScoreBoardManager Instance { get; private set; }

    private NetworkList<PlayerStats> networkPlayerList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            networkPlayerList = new NetworkList<PlayerStats>();
        }
        else
        { 
            Destroy(gameObject);  
        }
        
        //networkPlayerList.OnListChanged += OnPlayerListChanged;
    }
    //private void OnPlayerListChanged(NetworkListEvent<PlayerStats> changeEvent)
    //{
    //    List<PlayerStats> playerList = new List<PlayerStats>();

    //    foreach (var player in networkPlayerList)
    //        playerList.Add(player);
    //    ScoreboardUI.Instance.UpdateScoreboard(playerList);
    //}

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandlePlayerConnected;
        }
    }
    //public override void OnNetworkDespawn()
    //{
    //    if (IsServer)
    //    {
    //        NetworkManager.Singleton.OnClientConnectedCallback -= HandlePlayerConnected;
    //    }
    //}

    private void HandlePlayerConnected(ulong playerId)
    {
        Debug.Log($"<color=purple>Pozvan sam</color>");
        string playerNameString = EditPlayerName.Instance.GetPlayerName();

        Debug.Log($"<color=purple>Server: Player connected with ID: {playerId}</color>");

        networkPlayerList.Add(new PlayerStats
        {
            playerId = playerId,
            playerName = new FixedString32Bytes(playerNameString),
            CurrScore = 0,
            TotalScore = 0
        });

        Debug.Log($"<color=purple>Added new player to list. New count: {networkPlayerList.Count}</color>");
        foreach (var player in networkPlayerList)
        {
            Debug.Log($"<color=purple>Player:{player.playerId}  Name: {player.playerName} Tscore: {player.TotalScore}</color>");
        }
    }

}
