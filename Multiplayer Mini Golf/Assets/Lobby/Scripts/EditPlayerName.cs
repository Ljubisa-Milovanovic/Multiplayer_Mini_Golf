using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// No NetworkBehaviour needed for a purely local UI script
public class EditPlayerName : MonoBehaviour
{
    public static EditPlayerName Instance { get; private set; }

    // Event to notify when the name is changed locally
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

        // Ensure the button is assigned in the Inspector
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
        // Initial setup for the lobby manager (if LobbyManager expects this)
        // You might want to delay this until NetworkManager is ready,
        // or have LobbyManager subscribe to OnLocalPlayerNameChanged directly.
        // If LobbyManager is a NetworkBehaviour, it needs to be spawned first.
        // LobbyManager.Instance.UpdatePlayerName(GetPlayerName()); // This will be handled differently now
    }

    private void ShowNameInputDialog()
    {
        UI_InputWindow.Show_Static("Player name", playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ_-", 20,
        () => {
            // Cancel
        },
        (string newName) => {
            playerName = newName;
            playerNameText.text = playerName;
            // Invoke the event when the local player name changes
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
        // This is now purely local and doesn't care about network ownership
        return playerName;
    }
}