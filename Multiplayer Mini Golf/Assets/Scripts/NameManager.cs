using QFSW.QC;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NameManager : NetworkBehaviour
{
    public static NameManager instance { get; private set; }

    public NetworkList<PlayerStats> networkPlayerList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        networkPlayerList = new NetworkList<PlayerStats>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializePlayerList();
            //AddPlayerToList(0, "ljuba");
        }
    }

    private void InitializePlayerList()
    {
        if (networkPlayerList.Count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                networkPlayerList.Add(new PlayerStats
                {
                    playerId = ulong.MaxValue,
                    playerName = new FixedString32Bytes("NN"),
                    CurrScore = 0,
                    TotalScore = 0
                });
            }
        }
        Debug.Log("inicializovan sam");
    }


    [Command("AddPlayersToListFromClient")]
    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerToListServerRpc(ulong playerId, string playerName)
    {
        AddPlayerToList(playerId, playerName); // call the shared helper
    }

    [Command("AddPlayersToList")]
    public void AddPlayerToList(ulong playerId, string playerName)
    {
        if (networkPlayerList.Count == 0)
        {
            InitializePlayerList();
        }

        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            if (networkPlayerList[i].playerId == ulong.MaxValue)
            {
                networkPlayerList[i] = new PlayerStats
                {
                    playerId = playerId,
                    playerName = new FixedString32Bytes(playerName),
                    CurrScore = 0,
                    TotalScore = 0
                };
                ScoreBoardManager.Instance.FillInNamesBoard();
                ScoreBoardManager.Instance.FillInNameTab();
                IspisiListuSvihIgraca();
                return;
            }
        }

        Debug.LogWarning($"No empty slot available! Count = {networkPlayerList.Count}");
    }

    [Command("IspisiSveIgrace")]
    private void IspisiListuSvihIgraca()
    {
        Debug.Log("<color=blue>--- Current Player List ---</color>");
        foreach (var player in networkPlayerList)
        {
            Debug.Log($"<color=purple>Player:{player.playerId}  Name: {player.playerName} Tscore: {player.TotalScore}</color>");
        }
        Debug.Log("<color=blue>-------------------------</color>");
    }







    [ServerRpc(RequireOwnership = false)] // RequireOwnership=false allows any client to request an update for any player,
                                          // but you might want to adjust this based on your game's logic.
                                          // If only the player themselves can update their score, keep it true.
    public void UpdatePlayerTotalScoreServerRpc(ulong playerIdToUpdate, int scoreIncrease)
    {
        //if (!IsServer) return; // Only the server can modify the NetworkList

        Debug.Log($"<color=orange>ServerRpc: Attempting to update score for Player ID: {playerIdToUpdate} by {scoreIncrease}</color>");

        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            PlayerStats playerStats = networkPlayerList[i];
            if (playerStats.playerId == playerIdToUpdate)
            {
                playerStats.TotalScore += scoreIncrease;
                // You MUST reassign the struct back to the NetworkList at the same index
                // because structs are value types. Modifying it directly won't trigger
                // the NetworkList's change detection.
                networkPlayerList[i] = playerStats;
                Debug.Log($"<color=orange>Server: Updated score for Player ID: {playerIdToUpdate}. New TotalScore: {playerStats.TotalScore}</color>");
                ScoreBoardManager.Instance.FillInTotalScoresBoard();
                ScoreBoardManager.Instance.FillInTotalScoreTab();
                return;
            }
        }
        Debug.LogWarning($"<color=orange>Server: Player with ID {playerIdToUpdate} not found in list for score update.</color>");
    }
}
