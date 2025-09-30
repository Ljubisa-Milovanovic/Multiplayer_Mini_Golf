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
        networkPlayerList.OnListChanged += OnPlayerListChanged;
    }

    private void OnPlayerListChanged(NetworkListEvent<PlayerStats> changeEvent)
    {
        Debug.Log($"Player list changed: {changeEvent.Type}");

        // Refresh the UI (both server & clients will run this)
        if (ScoreBoardManager.Instance != null)
        {
            ScoreBoardManager.Instance.FillInNamesBoard();
            ScoreBoardManager.Instance.FillInNameTab();
            ScoreBoardManager.Instance.FillInTotalScoresBoard();
            ScoreBoardManager.Instance.FillInTotalScoreTab();
            ScoreBoardManager.Instance.FillInHole();
            switch (GameMenager.instance.BrojNivoa-1)// mzd -1 
            {
                case 1:
                    ScoreBoardManager.Instance.FillColumnOne();
                    break;
                case 2:
                    ScoreBoardManager.Instance.FillColumnTwo();
                    break;
                case 3:
                    ScoreBoardManager.Instance.FillColumnThree();
                    break;
                case 4:
                    ScoreBoardManager.Instance.FillColumnFour();
                    break;
                default:
                    break;

            }
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
                    TotalScore = 0,
                    HoleNumber = 0
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
                    TotalScore = 0,
                    HoleNumber = 1
                };
                //ScoreBoardManager.Instance.FillInNamesBoard();
                //ScoreBoardManager.Instance.FillInNameTab();
                //ScoreBoardManager.Instance.FillInHole();
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


    public void ResetCurrScore()
    {
        networkPlayerList.OnListChanged -= OnPlayerListChanged;
        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            PlayerStats playerStats = networkPlayerList[i];
            playerStats.CurrScore = 0;
            networkPlayerList[i] = playerStats;
        }
        networkPlayerList.OnListChanged += OnPlayerListChanged;
        GameMenager.instance.CurrShouldBeReset = false;
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
                playerStats.CurrScore += scoreIncrease;
                networkPlayerList[i] = playerStats;
                Debug.Log($"<color=orange>Server: Updated score for Player ID: {playerIdToUpdate}. New TotalScore: {playerStats.TotalScore}</color>");
                //ScoreBoardManager.Instance.FillInTotalScoresBoard();
                //ScoreBoardManager.Instance.FillInTotalScoreTab();
                return;
            }
        }
        Debug.LogWarning($"<color=orange>Server: Player with ID {playerIdToUpdate} not found in list for score update.</color>");
    }

    [ServerRpc(RequireOwnership = false)]
    public void HoleUpdateServerRpc(ulong playerId)
    {
        Debug.Log("<color=blue>Hole update server rpc pozvan</color>");
        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            PlayerStats playerStats = networkPlayerList[i];
            if (playerStats.playerId != ulong.MaxValue)
            {
                playerStats.HoleNumber = GameMenager.instance.BrojNivoa-1;
                
                if (playerStats.playerId != playerId)
                {
                    playerStats.TotalScore += 5;
                    playerStats.CurrScore +=5;
                }
                networkPlayerList[i] = playerStats;
                Debug.Log($"<color=orange>Server: Updated hole for Player ID: {playerId}. New HoleNumber: {playerStats.HoleNumber}</color>");
            }
        }
       // ScoreBoardManager.Instance.FillInHole();

        /*switch (GameMenager.instance.BrojNivoa-2)
        {
            case 1:
                ScoreBoardManager.Instance.FillColumnOne();
                break;
            case 2:
                ScoreBoardManager.Instance.FillColumnTwo();
                break;
            case 3:
                ScoreBoardManager.Instance.FillColumnThree();
                break;
            case 4:
                ScoreBoardManager.Instance.FillColumnFour();
                break;
            default:
                break;

        }*/
        ResetCurrScore();

        if (ScoreBoardManager.Instance != null)
        {
            ScoreBoardManager.Instance.FillInNamesBoard();
            ScoreBoardManager.Instance.FillInNameTab();
            ScoreBoardManager.Instance.FillInTotalScoresBoard();
            ScoreBoardManager.Instance.FillInTotalScoreTab();
            ScoreBoardManager.Instance.FillInHole();

            switch (GameMenager.instance.BrojNivoa - 1)
            {
                case 1: ScoreBoardManager.Instance.FillColumnOne(); break;
                case 2: ScoreBoardManager.Instance.FillColumnTwo(); break;
                case 3: ScoreBoardManager.Instance.FillColumnThree(); break;
                case 4: ScoreBoardManager.Instance.FillColumnFour(); break;
            }
        }
        Debug.LogWarning($"<color=orange>Server: Player with ID {playerId} not found in list for hole update.</color>");
    }

    private void OnDestroy()
    {
        if (networkPlayerList != null)
        {
            networkPlayerList.OnListChanged -= OnPlayerListChanged;
        }
    }

    public string getNajveciScore()
    {
        int max = 0;
        ulong maxId = ulong.MaxValue;
        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            if (networkPlayerList[i].TotalScore > max)
            {
                maxId = networkPlayerList[i].playerId;
            }
        }
        for (int i = 0; i < networkPlayerList.Count; i++)
        {
            if (networkPlayerList[i].playerId == maxId)
            {
                return networkPlayerList[i].playerName.ToString();
            }
        }
        return networkPlayerList[0].playerName.ToString();
    }

}
