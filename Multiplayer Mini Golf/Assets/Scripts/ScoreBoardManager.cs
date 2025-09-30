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
    public TextMeshProUGUI[] Holes;
    public TextMeshProUGUI[] ColumnOne;
    public TextMeshProUGUI[] ColumnTwo;
    public TextMeshProUGUI[] ColumnThree;
    public TextMeshProUGUI[] ColumnFour;
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



    [Command("FillInHole")]
    public void FillInHole()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                Holes[i].text = NameManager.instance.networkPlayerList[i].HoleNumber.ToString();
            }
            else
            {
                Holes[i].text = "";
            }
        }
    }


    [Command("FillColumnOne")]
    public void FillColumnOne()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                ColumnOne[i].text = NameManager.instance.networkPlayerList[i].CurrScore.ToString();
            }
            else
            {
                ColumnOne[i].text = "";
            }
        }
    }

    [Command("FillColumnTwo")]
    public void FillColumnTwo()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                ColumnTwo[i].text = NameManager.instance.networkPlayerList[i].CurrScore.ToString();
            }
            else
            {
                ColumnTwo[i].text = "";
            }
        }
    }

    [Command("FillColumnThree")]
    public void FillColumnThree()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                ColumnThree[i].text = NameManager.instance.networkPlayerList[i].CurrScore.ToString();
            }
            else
            {
                ColumnThree[i].text = "";
            }
        }
    }

    [Command("FillColumnFour")]
    public void FillColumnFour()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < NameManager.instance.networkPlayerList.Count)
            {
                ColumnFour[i].text = NameManager.instance.networkPlayerList[i].CurrScore.ToString();
            }
            else
            {
                ColumnFour[i].text = "";
            }
        }
    }



}