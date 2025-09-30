using System.Collections;
using System.Collections.Generic;
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
            // find any Canvas in children (even inactive)
            targetCanvas = GetComponentInChildren<Canvas>(true);
        }
        if (targetCanvas == null)
            Debug.LogWarning("CanvasToggle: No Canvas found in children.");
    }

    public void Show()
    {
        if (targetCanvas != null) targetCanvas.enabled = true;
    }

    public void Hide()
    {
        if (targetCanvas != null) targetCanvas.enabled = false;
    }

    public void SetPobednikText(string text)
    {
        Pobednik.text = $"Pobedio je {text}";
    }
}
