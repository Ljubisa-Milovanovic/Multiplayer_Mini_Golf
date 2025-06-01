using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {

    public static AuthenticateUI Instance { get; private set; }

    [SerializeField] private Button authenticateButton;


    private void Awake() {
        Instance = this;
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
            EditPlayerName.Instance.DisableNameEdit();
        });
    }

    public void Hide() {
        gameObject.SetActive(false);
        Debug.Log("hiding authentication ui");
    }

}