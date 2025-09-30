using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EditPlayerName : MonoBehaviour
{
    public static EditPlayerName Instance { get; private set; }

    
    public event Action<string> OnLocalPlayerNameChanged;

    [SerializeField] private TextMeshProUGUI playerNameText;

    private string playerName = "miniGolfer";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        
        Button nameEditButton = GetComponent<Button>();
        if (nameEditButton != null)
        {
            nameEditButton.onClick.AddListener(ShowNameInputDialog);
        }
        else
        {
            Debug.LogError("EditPlayerName: No Button component found on this GameObject.", this);
        }

        playerNameText.text = playerName;
    }

    private void Start()
    {
      
    }

    private void ShowNameInputDialog()
    {
        UI_InputWindow.Show_Static("Player name", playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ_-", 20,
        () => {
            
        },
        (string newName) => {
            playerName = newName;
            playerNameText.text = playerName;
            
            OnLocalPlayerNameChanged?.Invoke(playerName);
        });
    }

    public void DisableNameEdit()
    {
        Button nameEditButton = GetComponent<Button>();
        if (nameEditButton != null)
        {
            nameEditButton.onClick.RemoveAllListeners();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public string GetPlayerName()
    {
        
        return playerName;
    }
}