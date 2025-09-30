using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public static Relay Instance { get; private set; }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    
    public async Task<string> CreateRelay()
    {
        Debug.Log("in the createRelay function");
        try
        {
            Debug.Log("starting relay host");
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); 
            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e) { 
            Debug.Log("e.Message = " + e.Message + " e = "+ e);
            return null;
        }
        
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("starting relay client with code" + joinCode);
            JoinAllocation JoinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(JoinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); 
            NetworkManager.Singleton.StartClient();    
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e.Message);
        }

    }
}
