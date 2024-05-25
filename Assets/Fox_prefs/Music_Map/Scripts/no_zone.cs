
using System.ComponentModel;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class no_zone : UdonSharpBehaviour
{

    bool inside = false;
    public music_sys sys;
    public void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = true;
            //menu_anim.SetBool("Inside", inside);
            sys.bocina.mute = inside;
        }

    }
    public void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = true;
            //menu_anim.SetBool("Inside", inside);
            sys.bocina.mute = inside;
        }
    }

    public void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = false;
            //menu_anim.SetBool("Inside", inside);
            sys.bocina.mute = inside;
        }
    }
}
