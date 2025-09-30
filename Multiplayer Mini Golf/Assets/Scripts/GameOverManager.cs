using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Pobednik;
    [SerializeField] private Canvas targetCanvas;
    public static GameOverManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (targetCanvas == null)
        {
            targetCanvas = GetComponentInChildren<Canvas>(true);
            Debug.Log($"Found canvas: {(targetCanvas != null ? targetCanvas.gameObject.name : "NULL")}");
        }
    }

    [Command("ShowCanvas")]
    public void Show()
    {
        if (targetCanvas != null) targetCanvas.gameObject.SetActive(true);
    }

    [Command("HideCanvas")]
    public void Hide()
    {
        if (targetCanvas != null) targetCanvas.gameObject.SetActive(false);
    }

    public void SetPobednikText(string text)
    {
        Pobednik.text = $"Pobedio je {text}";
    }
}
