
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class U_HashStudiosCentralSpanish_DoorInteract : UdonSharpBehaviour
{
    /// <summary>
    /// Copyright (c) 2024 Hash Studios LLC. All rights reserved.
    /// This code is the sole property of Hash Studios LLC and is protected by copyright laws.
    /// It may not be used, reproduced, or distributed without the express written permission of Hash Studios LLC.
    ///
    /// Any unauthorized use of this code is strictly prohibited and may result in legal action.
    ///
    /// This copyright notice must be included in any copies or reproductions of this code.
    ///
    /// NOTE: This copyright notice does not apply to any dependencies or
    /// external libraries used in this code, such as UdonSharp, VRC SDK, or VRChat.
    /// The copyright and licensing terms of these dependencies apply to their respective code.
    /// Hash Studios LLC is not claiming any rights to these dependencies.
    /// </summary>
    
    public U_HashStudiosCentralSpanishDoor_Internal internalScript;
    public bool isOutSideDoor;
    public Transform locationToTeleportTo;

    public override void Interact()
    {
        if(isOutSideDoor == true)
        {
            if(internalScript.isLocked == false)
            {
                Networking.LocalPlayer.TeleportTo(locationToTeleportTo.position, locationToTeleportTo.rotation);
            }
            else
            {
                if(internalScript.mainScript.isVIP == true)
                {
                    if (isPlayerVIP(Networking.LocalPlayer.displayName) == true)
                    {
                        Networking.LocalPlayer.TeleportTo(locationToTeleportTo.position, locationToTeleportTo.rotation);
                    }
                }
            }
        }
        else
        {
            Networking.LocalPlayer.TeleportTo(locationToTeleportTo.position, locationToTeleportTo.rotation);
        }
    }

    public bool isPlayerVIP(string displayName)
    {
        foreach(string str in internalScript.mainScript.vipMembers)
        {
            if (str.Equals(displayName))
            {
                return true;
            }
        }
        return false;
    }
}
