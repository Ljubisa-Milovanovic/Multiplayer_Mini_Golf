using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public struct PlayerStats : INetworkSerializable, IEquatable<PlayerStats>
{
    public ulong playerId;
    public FixedString32Bytes playerName;
    public int CurrScore;
    public int TotalScore;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref CurrScore);
        serializer.SerializeValue(ref TotalScore);
    }
    public bool Equals(PlayerStats other)
    {
        return playerId == other.playerId;//&& PlayerName == other.PlayerName
    }
}
