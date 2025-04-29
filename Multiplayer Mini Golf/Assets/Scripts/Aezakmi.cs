using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QFSW.QC;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[CommandPrefix("aezakmi.")]
public class Aezakmi : NetworkBehaviour
{
    public static Aezakmi Instance { get; private set; }

    private Vector3 spawnPosition = new Vector3(-1.5f, 3, -10);

    private System.Random rnd = new System.Random();

    [Command("tp")]
    private void TeleportBall(double a, double b, double c)
    {
        if (IsOwner)
        {
            Debug.Log("<color=red>Ball before tp :</color>" + transform.position);
            Vector3 teleportPostion = new Vector3((float)a, (float)b, (float)c);
            transform.position = teleportPostion;
            Debug.Log("<color=green>Ball tp-ed to :</color>" + transform.position + ", tp position : " + teleportPostion);
        }
    }

    public void SpawnPoint()
    {
        if (IsOwner)
        {
            spawnPosition += new Vector3((float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3), 0, (float)Math.Round(rnd.NextDouble() * 0.5 + 0.25, 3));
            Debug.Log("<color=red>Ball position before setting:</color>" + transform.position + ", spawn position : " + spawnPosition);
            transform.position = spawnPosition;
            Debug.Log("<color=green>Ball position after setting:</color>" + transform.position + ", spawn position : " + spawnPosition);
        }
    }
}