
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class bedmirror_reset : UdonSharpBehaviour
{

    [Space(15)]
    [SerializeField]
    GameObject[] Mirrors;
    [Space(15)]
    [SerializeField]
    Image[] BotonSwitches;


    [Space(15)]
    [SerializeField]
    Sprite SwitchOff;
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            for (int i = 0; i < BotonSwitches.Length; i++)
            {
                BotonSwitches[i].sprite = SwitchOff;
                Mirrors[i].SetActive(false);
            }
        }
    }
}
