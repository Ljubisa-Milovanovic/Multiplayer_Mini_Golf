using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QFSW.QC;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components; 
using UnityEngine;

[CommandPrefix("aezakmi.")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ClientNetworkTransform))] 
public class Aezakmi : NetworkBehaviour 
{

    
    private Rigidbody _rigidbody;
    private Vector3 spawnPositionBase = new Vector3(0, 1, -10);
    private System.Random rnd = new System.Random();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) { /* Error log */ }

        
    }

    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); 

        
        if (IsOwner)
        {
            Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] Aezakmi - OnNetworkSpawn: I own this ball. Calling SpawnPoint().");
            SpawnPoint(); 
        }
        else
        {
            Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] Aezakmi - OnNetworkSpawn: I do NOT own this ball. Position will be synced.");
            
        }
    }


    [Command("tp")]
    public void TeleportBall(double a, double b, double c)
    {
        if (!IsOwner || _rigidbody == null) return;

        Debug.Log("<color=orange>Ball Rigidbody position BEFORE teleport command:</color>" + _rigidbody.position);
        Vector3 teleportPosition = new Vector3((float)a, (float)b, (float)c);

        _rigidbody.MovePosition(teleportPosition);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Debug.Log("<color=green>Ball Rigidbody position set to:</color>" + _rigidbody.position + ", Target: " + teleportPosition);
    }

    
    public void SpawnPoint() 
    {
        
        if (!IsOwner || _rigidbody == null)
        {
            
            return;
        }

        Vector3 randomOffset = new Vector3(
            //(float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3),
            0,0,0
            //(float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3)
        );
        Vector3 targetSpawnPosition = spawnPositionBase + randomOffset;

        Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] <color=orange>Executing SpawnPoint. Rigidbody position BEFORE set:</color>" + _rigidbody.position + ", target spawn position: " + targetSpawnPosition);

        _rigidbody.MovePosition(targetSpawnPosition);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] <color=green>Executed SpawnPoint. Rigidbody position set for spawn:</color>" + _rigidbody.position);
    }

    
}