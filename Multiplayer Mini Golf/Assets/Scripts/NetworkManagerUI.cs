using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ClientBtn;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        });
        ClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
