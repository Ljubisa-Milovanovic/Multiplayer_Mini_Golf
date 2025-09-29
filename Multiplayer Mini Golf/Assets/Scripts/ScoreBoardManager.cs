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

    public NetworkList<PlayerStats> networkPlayerList;

    private void Awake()
    {      
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("im awake");
        networkPlayerList = new NetworkList<PlayerStats>();
    }

    private void OnPlayerListChanged(NetworkListEvent<PlayerStats> changeEvent)
    {
        // This will be called on all clients when the NetworkList changes on the server.
        // You can use this to update your UI.
        Debug.Log($"<color=green>NetworkList changed! Type: {changeEvent.Type}</color>");
        List<PlayerStats> playerList = new List<PlayerStats>();
        foreach (var player in networkPlayerList)
        {
            playerList.Add(player);
        }
        // Assuming you have a ScoreboardUI script that handles the display
        // ScoreboardUI.Instance.UpdateScoreboard(playerList);
        //IspisiListuIgraca(); // For debugging
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("spawnovo sam se");

        networkPlayerList.OnListChanged += OnPlayerListChanged;

        if (IsServer)
        {
            
            NetworkManager.Singleton.OnClientConnectedCallback += HandlePlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnected;
        }
    }

    


    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandlePlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnected;
        }
    }

    private void HandlePlayerConnected(ulong playerId)
    {
        Debug.Log($"<color=purple>Added new player to list. New count: {networkPlayerList.Count}</color>");
        //IspisiListuIgraca();
    }

    private void HandlePlayerDisconnected(ulong playerId)
    {
        Debug.Log($"<color=red>Player disconnected: {playerId}</color>");
        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            if (networkPlayerList[i].playerId == playerId)
            {
                networkPlayerList[i] = new PlayerStats
                {
                    playerId = ulong.MaxValue,
                    playerName = new FixedString32Bytes(""),
                    CurrScore = 0,
                    TotalScore = 0
                };

                Debug.Log($"<color=red>Cleared slot {i} after player left</color>");
                break;
            }
        }
    }


    // ServerRpc to update a player's score
    


   

    

}