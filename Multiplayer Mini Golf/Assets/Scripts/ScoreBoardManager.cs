using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using QFSW.QC;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoardManager : NetworkBehaviour
{
    public TextMeshProUGUI[] namesBoard;
    public TextMeshProUGUI[] namesTab;
    public TextMeshProUGUI[] TotalScoresBoard;
    public TextMeshProUGUI[] TotalScoresTab;
    public static ScoreBoardManager Instance { get; private set; }

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
    }

    [Command("FillInNamesBoard")]
    public void FillInNamesBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                namesBoard[i].text = NameManager.instance.networkPlayerList[i].playerName.ToString();
            }
            else
            {
                namesBoard[i].text = "";
            }
        }
    }

    [Command("FillInNameTab")]
    public void FillInNameTab()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                namesTab[i].text = NameManager.instance.networkPlayerList[i].playerName.ToString();
            }
            else
            {
                namesTab[i].text = "";
            }
        }
    }

    [Command("FillInTotalScoresBoard")]
    public void FillInTotalScoresBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                TotalScoresBoard[i].text = NameManager.instance.networkPlayerList[i].TotalScore.ToString();
            }
            else
            {
                TotalScoresBoard[i].text = "";
            }
        }
    }

    [Command("FillInTotalScoreTab")]
    public void FillInTotalScoreTab()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                TotalScoresTab[i].text = NameManager.instance.networkPlayerList[i].TotalScore.ToString();
            }
            else
            {
                TotalScoresTab[i].text = "";
            }
        }
    }











}