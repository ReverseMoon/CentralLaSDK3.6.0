
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using UnityEngine.UI;

public class U_HashStudiosCentralSpanishDoor_Internal : UdonSharpBehaviour
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
    
    public U_HashStudiosCentralSpanishDoor_Main mainScript;

    public TextMeshProUGUI [] roomName;

    public TextMeshProUGUI[] playerCount;

    public TextMeshProUGUI[] lockCheck;

    public TextMeshProUGUI textDoNotDisturb;

    public Sprite toggleOn;

    public Sprite toggleOff;

    public Image img_toggleLocked;

    public Image img_showPlayerCount;

    public Image img_doNotDisturb;

    public Image img_anonymizePlayers;

    public U_HashStudiosCentralSpanishDoor_DetectPlayers detectPlayersScript;

    [UdonSynced, FieldChangeCallback(nameof(isLocked_Update))] public bool isLocked;

    [UdonSynced, FieldChangeCallback(nameof(showPlayerCount_Update))] public bool showPlayerCount;

    [UdonSynced, FieldChangeCallback(nameof(doNotDisturb_Update))] public bool doNotDisturb;

    [UdonSynced, FieldChangeCallback(nameof(anonymizePlayers_Update))] public bool anonymizePlayers;

    public int numberOfPlayersInside;

    public bool isLocked_Update
    {
        set
        {
            isLocked = value;
            checkLock();
        }
        get => isLocked;
    }

    public bool showPlayerCount_Update
    {
        set
        {
            showPlayerCount = value;
            checkPlayerCount();
        }
        get => showPlayerCount;
    }

    public bool doNotDisturb_Update
    {
        set
        {
            doNotDisturb = value;
            checkDoNotDisturb();
        }
        get => doNotDisturb;
    }

    public bool anonymizePlayers_Update
    {
        set
        {
            anonymizePlayers = value;
            checkAnonymization();
        }
        get => anonymizePlayers;
    }

    public void Start()
    {
        foreach(TextMeshProUGUI tm in roomName)
        {
            tm.text = mainScript.roomName;
        }
        checkLock();
        checkDoNotDisturb();
        checkPlayerCount();
        checkAnonymization();
        //Debug.LogError("PLAYER ID: " + Networking.LocalPlayer.playerId);
        if(mainScript.isVIP == true && Networking.LocalPlayer.playerId == 1){
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            SetProgramVariable(nameof(isLocked), true);
            checkLock();
        }
    }

    public void setCounter(int i)
    {
        foreach(TextMeshProUGUI tm in playerCount)
        {
            tm.text = "" + numberOfPlayersInside;
        }
    }

    public void toggleLock()
    {
        if(mainScript.isVIP == true)
        {
            if (isPlayerVIP(Networking.LocalPlayer.displayName) == true)
            {
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                SetProgramVariable(nameof(isLocked), !isLocked);
                checkLock();
            }
        }
        else
        {
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            SetProgramVariable(nameof(isLocked), !isLocked);
            checkLock();
        }
        
    }

    public void checkLock()
    {
        if(isLocked == true)
        {
            img_toggleLocked.sprite = toggleOn;
            foreach (TextMeshProUGUI tm in lockCheck)
            {
                tm.text = "Bloqueado";
            }
        }
        else
        {
            img_toggleLocked.sprite = toggleOff;
            foreach (TextMeshProUGUI tm in lockCheck)
            {
                tm.text = "Desbloqueado";
            }
        }
    }

    public void toggleDoNotDisturb()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SetProgramVariable(nameof(doNotDisturb), !doNotDisturb);
        checkDoNotDisturb();
    }

    public void checkDoNotDisturb()
    {
        if (doNotDisturb == true)
        {
            img_doNotDisturb.sprite = toggleOn;
            textDoNotDisturb.text = "No Interrumpir~";
        }
        else
        {
            img_doNotDisturb.sprite = toggleOff;
            textDoNotDisturb.text = "Por Favor Toca el Timbre <3";
        }
    }

    public void toggleShowPlayerCount()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SetProgramVariable(nameof(showPlayerCount), !showPlayerCount);
        checkPlayerCount();
    }

    public void checkPlayerCount()
    {
        detectPlayersScript.updateText();
        if (showPlayerCount == false)
        {
            img_showPlayerCount.sprite = toggleOn;
        }
        else
        {
            img_showPlayerCount.sprite = toggleOff;
        }
    }

    public void toggleAnonymizePlayers()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SetProgramVariable(nameof(anonymizePlayers), !anonymizePlayers);
        checkAnonymization();
    }

    public void checkAnonymization()
    {
        detectPlayersScript.updateText();
        if (anonymizePlayers == true)
        {
            img_anonymizePlayers.sprite = toggleOn;
        }
        else
        {
            img_anonymizePlayers.sprite = toggleOff;
        }
    }

    public bool isPlayerVIP(string displayName)
    {
        foreach(string str in mainScript.vipMembers)
        {
            if (str.Equals(displayName))
            {
                return true;
            }
        }
        return false;
    }

}
