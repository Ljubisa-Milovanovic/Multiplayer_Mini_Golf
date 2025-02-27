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
        // Singleton pattern: Ensure only one instance exists.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy this instance if another already exists.
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep the Relay object alive between scenes.  Essential!
        //AuthenticationService.Instance.SignedIn += () =>
        //{
        //    Debug.Log("Singed in " + AuthenticationService.Instance.PlayerName + " : " + AuthenticationService.Instance.PlayerId);
        //};
        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    //have to delete main camera on create relay or remove its tag
    public async Task<string> CreateRelay()
    {
        Debug.Log("in the createRelay function");
        try
        {
            Debug.Log("starting relay host");
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);//7 je max connections jer se host ne racuna pa je 8 -> 7
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // mzd umesto unityTransport treba relayserver transport
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
            JoinAllocation JoinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);//3 je max connections jer se host ne racuna pa je 4 -> 3

            RelayServerData relayServerData = new RelayServerData(JoinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // mzd umesto unityTransport treba relayserver transport
            NetworkManager.Singleton.StartClient();    
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e.Message);
        }

    }
}
