
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class U_HashStudiosCentralSpanishDoor_Doorbell : UdonSharpBehaviour
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

    public AudioSource[] audioSource_doorBellRing;

    public bool disabled;

    [UdonSynced, FieldChangeCallback(nameof(doorBell_Update))] public int doorBellInt;

    public override void Interact()
    {
        if(internalScript.doNotDisturb == false && disabled == false)
        {
            ringDoorbell();
        }
    }

    public int doorBell_Update
    {
        set
        {
            playDoorbellSound();
            doorBellInt = value;

            disabled = true;
            SendCustomEventDelayedSeconds(nameof(enableDoorbell), 5f);
        }
        get => doorBellInt;
    }

    public void ringDoorbell()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        int val = doorBellInt + 1;
        if (val > 3)
        {
            val = 0;
        }
        Mathf.Clamp(val, 0, 3);
        SetProgramVariable(nameof(doorBellInt), val);
        playDoorbellSound();

        disabled = true;
        SendCustomEventDelayedSeconds(nameof(enableDoorbell), 5f);
    }

    public void enableDoorbell(){
        disabled = false;
    }

    public void playDoorbellSound()
    {
        if(disabled == false)
        {
            foreach (AudioSource src in audioSource_doorBellRing)
            {
                src.Play();
            }
        }
    }
}
