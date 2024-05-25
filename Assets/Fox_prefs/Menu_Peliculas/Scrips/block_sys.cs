
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.SDK3.Components;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class block_sys : UdonSharpBehaviour
{
    public Animator timer_menu;
    public Text timer_text;
    public int secs_to_wait=180;

    [UdonSynced(UdonSyncMode.None)] private int sync_secs=180;
    private int min, sec, secs_rest;

    [UdonSynced(UdonSyncMode.None)] private bool sync_done = true;
    public bool done = true;

    public GameObject obj_padre, obj_padre_seri;
    public Button[] botones;
    public VRCUrlInputField url_2_block;

    public GameObject to_disable;

    public void _do_block_master() {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        _do_block();
    }

    public void _do_block() {
        secs_rest = secs_to_wait;
        //timer_text.text = "Tiempo de espera\n3:00";
        timer_text.text = "3:00";

        done = false;
        timer_menu.SetBool("Done", done);
        _do_countdown();
        //SendCustomEventDelayedSeconds("_close", secs_rest);

        foreach (Transform child in obj_padre.transform)
        {
            child.gameObject.GetComponent<Button>().interactable=false;
        }
        foreach (Transform child in obj_padre_seri.transform)
        {
            child.gameObject.GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].interactable = false;
        }
        url_2_block.interactable=false;
        to_disable.SetActive(false);

        if (Networking.LocalPlayer.IsOwner(gameObject))
        {
            sync_done = done;
            RequestSerialization();
        }
    }

    public void _do_countdown() {
        if (secs_rest <= 0)
        {
            secs_rest = secs_to_wait;
            //timer_text.text = "Tiempo de espera\n0:00";
            timer_text.text = "0:00";
            _close();
        }
        else if (secs_rest != 0) {
            min = secs_rest / 60;
            sec = secs_rest % 60;
            //min = min % 60;
            
            //if (sec < 10) { timer_text.text = "Tiempo de espera\n" + min + ":0" + sec; }
            //else { timer_text.text = "Tiempo de espera\n" + min + ":" + sec; }
            if (sec < 10) { timer_text.text = min + ":0" + sec; }
            else { timer_text.text = min + ":" + sec; }

            secs_rest--;
            SendCustomEventDelayedSeconds("_do_countdown", 1);
        }

        if (Networking.LocalPlayer.IsOwner(gameObject))
        {
            sync_secs = secs_rest;
            RequestSerialization();
        }

    }

    public void _close() {
        done = true;
        //timer_menu.Play("Close");
        timer_menu.SetBool("Done", done);

        foreach (Transform child in obj_padre.transform)
        {
            child.gameObject.GetComponent<Button>().interactable = true;
        }
        foreach (Transform child in obj_padre_seri.transform)
        {
            child.gameObject.GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].interactable = true;
        }
        url_2_block.interactable = true;
        to_disable.SetActive(true);

        if (Networking.LocalPlayer.IsOwner(gameObject))
        {
            sync_done = done;
            RequestSerialization();
        }
    }

    public virtual void OnDeserialization()
    {
        if (done != sync_done) { 
            done = sync_done;
            if (done == false) { 
                _do_block();
                if (secs_rest != sync_secs)
                {
                    secs_rest = sync_secs;
                    _do_countdown();
                }
            }
            //if (done = true) { _close(); }

            
        }

        
    }
}
