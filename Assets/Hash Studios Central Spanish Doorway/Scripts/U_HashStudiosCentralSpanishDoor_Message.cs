
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.SDK3.Components;
using TMPro;

public class U_HashStudiosCentralSpanishDoor_Message : UdonSharpBehaviour
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

    public Animator messageAnimator;

    public TextMeshProUGUI messageToDisplay;

    public Text textOfMessage;

    [UdonSynced, FieldChangeCallback(nameof(messageText_Update))] public string messageText;

    public string messageText_Update
    {
        set
        {
            messageText = value;
            sendMessage();
        }
        get => messageText;
    }

    public void finishMessage()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SetProgramVariable(nameof(messageText), textOfMessage.text);
        sendMessage();
    }

    public void sendMessage()
    {
        if(messageText != null)
        {
            messageToDisplay.text = messageText;
        }
        messageAnimator.Play("Show");
        SendCustomEventDelayedSeconds(nameof(hideMessage), 3f);
    }

    public void hideMessage()
    {
        messageAnimator.Play("Hide");
    }
}
