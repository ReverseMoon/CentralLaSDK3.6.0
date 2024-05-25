
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class acercandose : UdonSharpBehaviour
{
    //public GameObject obj;
    public Animator menu_anim,notice;
    bool inside=false;
    public block_sys blocc_sys;

    public void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        inside = true;
        //if (player.isLocal == true) { obj.SetActive(true); }
        //if (player.isLocal == true) { menu_anim.Play("Open");}
        if (player.isLocal == true) {
            inside = true;
            menu_anim.SetBool("Inside", inside);

            if (blocc_sys.done==false) {
                notice.SetBool("Done", false);
            }
        }

    }
    public void OnPlayerTriggerStay(VRCPlayerApi player)
    {
       
        if (player.isLocal == true) {
            inside = true;
            menu_anim.SetBool("Inside", inside);

            if (blocc_sys.done == false)
            {
                notice.SetBool("Done", false);
            }
        }
    }

    public void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        //if (player.isLocal == true) { obj.SetActive(false); }
        //if (player.isLocal == true) { menu_anim.Play("Close"); }
        if (player.isLocal == true) {
            inside = false;
            menu_anim.SetBool("Inside", inside);

            if (blocc_sys.done == false)
            {
                notice.SetBool("Done", true);
            }
        }
    }
}
