using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class PlayerListSync : UdonSharpBehaviour
{
    [UdonSynced]
    public string Name;

    public PlayerList Controller;

    public void ManualUpdate()
    {
        var player = Networking.LocalPlayer;
        if (player.displayName != Name)
        {
            Debug.Log($"[PlayerListSync] set data for {player.displayName} #{player.playerId}");

            this.Name = player.displayName;
            this.RequestSerialization();
        }
    }
}