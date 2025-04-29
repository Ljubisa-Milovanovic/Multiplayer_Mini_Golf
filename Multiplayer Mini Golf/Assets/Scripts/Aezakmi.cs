using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QFSW.QC;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components; // For ClientNetworkTransform if needed
using UnityEngine;

[CommandPrefix("aezakmi.")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ClientNetworkTransform))] // Assuming you still use this
public class Aezakmi : NetworkBehaviour // Inherits from NetworkBehaviour
{

    // ... (Instance, spawnPositionBase, rnd, Rigidbody cache remain the same) ...
    private Rigidbody _rigidbody;
    private Vector3 spawnPositionBase = new Vector3(-1.5f, 3, -10.5f);//15 3 25
    private System.Random rnd = new System.Random();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) { /* Error log */ }

        // You might still want the Instance pattern if other scripts need Aezakmi
        // Be careful with static instances and network objects though.
        // Consider if Instance should only be set if IsOwner? Depends on usage.
        // if (Instance != null && Instance != this)
        // {
        //     Destroy(gameObject); // Or just the script component
        // }
        // else
        // {
        //     Instance = this;
        // }
    }

    // --- THIS IS THE NEW PART ---
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); // Good practice to call the base method

        // CRITICAL: Only the owner client should execute the SpawnPoint logic,
        // because it modifies the authoritative state (Rigidbody) for ClientNetworkTransform.
        if (IsOwner)
        {
            Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] Aezakmi - OnNetworkSpawn: I own this ball. Calling SpawnPoint().");
            SpawnPoint(); // Call the method within this script
        }
        else
        {
            Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] Aezakmi - OnNetworkSpawn: I do NOT own this ball. Position will be synced.");
            // Non-owners will receive the correct position via ClientNetworkTransform
            // automatically once the owner sets it and the transform syncs.
        }
    }
    // --- END OF NEW PART ---


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

    // Make SpawnPoint public if you intend to call it from elsewhere AFTER spawn,
    // but keep the IsOwner check inside it!
    // If it's ONLY for the initial spawn, private is fine if called from OnNetworkSpawn in this script.
    public void SpawnPoint() // Or keep private if only called internally
    {
        // The IsOwner check is essential here regardless of where it's called from
        if (!IsOwner || _rigidbody == null)
        {
            // Optional: Log if called incorrectly
            // if (_rigidbody != null) Debug.LogWarning($"SpawnPoint called on non-owner [{NetworkManager.Singleton.LocalClientId}]");
            return;
        }

        Vector3 randomOffset = new Vector3(
            (float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3),
            0,
            (float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3)
        );
        Vector3 targetSpawnPosition = spawnPositionBase + randomOffset;

        Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] <color=orange>Executing SpawnPoint. Rigidbody position BEFORE set:</color>" + _rigidbody.position + ", target spawn position: " + targetSpawnPosition);

        _rigidbody.MovePosition(targetSpawnPosition);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Debug.Log($"[{NetworkManager.Singleton.LocalClientId}] <color=green>Executed SpawnPoint. Rigidbody position set for spawn:</color>" + _rigidbody.position);
    }

    // Optional: Clean up static instance if you use it
    // public override void OnDestroy() // Or OnNetworkDespawn
    // {
    //     base.OnDestroy(); // Or base.OnNetworkDespawn();
    //     if (Instance == this)
    //     {
    //         Instance = null;
    //     }
    // }
}