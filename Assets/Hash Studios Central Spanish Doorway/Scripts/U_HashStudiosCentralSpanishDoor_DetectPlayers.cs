
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class U_HashStudiosCentralSpanishDoor_DetectPlayers : UdonSharpBehaviour
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

    public Transform playersParent;
    public Transform playerBox;

    public int[] allPlayers;

    public void Start()
    {
        playerBox.gameObject.SetActive(false);
    }

    public void updateText()
    {
        foreach(Transform child in playersParent)
        {
            if (!child.Equals(playerBox))
            {
                Destroy(child.gameObject);
            }
        }
        for(int i = 0; i < allPlayers.Length; i++)
        {
            GameObject newChild = Instantiate(playerBox.gameObject, playersParent);
            newChild.SetActive(true);
            if(internalScript.anonymizePlayers == true)
            {
                newChild.GetComponentInChildren<TextMeshProUGUI>().text = "########";
            }
            else
            {
                VRCPlayerApi temp = VRCPlayerApi.GetPlayerById(allPlayers[i]);
                newChild.GetComponentInChildren<TextMeshProUGUI>().text = temp.displayName;
            }
        }
        foreach(TextMeshProUGUI tm in internalScript.playerCount)
        {
            if(internalScript.showPlayerCount == false)
            {
                tm.text = "" + allPlayers.Length + " Jugadores";
            }
            else
            {
                tm.text = "[?]" + " Jugadores";
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(Find(player.playerId) == -1)
        {
            Add(player.playerId);
        }
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (Find(player.playerId) == -1)
        {
            Add(player.playerId);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (Find(player.playerId) != -1)
        {
            FindAndRemove(player.playerId);
        }
    }

    private int Find(int element)
    {
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] == element)
            {
                return i;
            }
        }

        return -1;
    }

    private void FindAndRemove(int element)
    {
        int index = Find(element);

        if (index != -1)
        {
            RemoveElementAtIndex(index);
        }
        else
        {
            Debug.Log("Element not found in array.");
        }
    }

    private void RemoveElementAtIndex(int index)
    {
        int[] newArray = new int[allPlayers.Length - 1];

        for (int i = 0, j = 0; i < allPlayers.Length; i++)
        {
            if (i != index)
            {
                newArray[j] = allPlayers[i];
                j++;
            }
        }

        allPlayers = newArray;

        Debug.Log("Element removed from array.");

        updateText();
    }

    private void Add(int element)
    {
        int[] newArray = new int[allPlayers.Length + 1];

        for (int i = 0; i < allPlayers.Length; i++)
        {
            newArray[i] = allPlayers[i];
        }

        newArray[allPlayers.Length] = element;

        allPlayers = newArray;

        Debug.Log("Element added to array.");

        updateText();
    }

}
