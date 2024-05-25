
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.SDK3.Components;
using VRC.Udon;
using UnityEngine.Playables;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]

public class music_sys : UdonSharpBehaviour
{

    public GameObject parnt_obj;
    public int song = 0, last_song, size;
    public AudioSource bocina;
    public bool playstat = true, loop_s = true, shuf_s = false;
    //public player_b info;
    public Animator menu_anim;
    bool inside = false, playing;

    public Sprite img_pau, img_pla;
    public UnityEngine.UI.Image but_play1, but_play2;
    //---------------------------------




    void Start()
    {
        size = parnt_obj.transform.childCount;
        //if (size > 1)
        //{
        //song = 0; last_song = size - 1;
        /*for (int i = 0; i < size; i++)
        {
            parnt_obj.transform.GetChild(i).GetComponent<Animator>().Play("GetOut");
        }*/
        SendCustomEventDelayedSeconds("do_cicle", 5);
        //do_cicle();
        //}
    }

    

    public void do_cicle()
    {
        //if (song == size) { song = 0; last_song = size - 1; }

        //parnt_obj.transform.GetChild(song).GetComponent<Animator>().Play("GetIn");
        //parnt_obj.transform.GetChild(last_song).GetComponent<Animator>().Play("GetOut");

        //song++;
        //last_song = song - 1;
        if (!bocina.isPlaying && playstat) { _next_song(); }
        SendCustomEventDelayedSeconds("do_cicle", 1);
    }

    public void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = true;
            menu_anim.SetBool("Inside", inside);
        }

    }
    public void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = true;
            menu_anim.SetBool("Inside", inside);
        }
    }

    public void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal == true)
        {
            inside = false;
            menu_anim.SetBool("Inside", inside);
        }
    }

    //public GameObject parnt_obj;
    //public int secs_a_esperar = 5;

    //public Sprite imagen;


    /*public void Start()
    {
        if (titulo.Length < 13)
        {
            gameObject.transform.GetChild(0).GetComponent<ScrollRect>().enabled = false;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = titulo;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "  " + titulo + "  ";
        }
    }*/

    public void _rep_but() {
        but_play1.sprite = img_pau;
        but_play2.sprite = img_pau;
        playstat = true;
    }

    public void _pl_pa_song()
    {
        playstat = !playstat;
        
        if (playstat == true ) {
            but_play1.sprite = img_pau;
            but_play2.sprite = img_pau;
            bocina.Play();
        }
        else if(playstat == false)
        {
            but_play1.sprite = img_pla;
            but_play2.sprite = img_pla;
            bocina.Pause();
        }
    }

    public void _next_song()
    {
        //last_song = size - 1; 
        
        song++;
        if (song == size) { song = 0; }
        bocina.Stop();
        //bocina.clip = parnt_obj.transform.GetChild(song).GetComponent<player_b>().track_song;
        parnt_obj.transform.GetChild(song).GetComponent<player_b>()._do_stuff_m();
        bocina.Play();
        playstat = true;

        //last_song = song - 1;
    }
    public void _prev_song()
    {
        

        song--;
        if (song < 0) { song = size-1; }
        bocina.Stop();
        bocina.clip = parnt_obj.transform.GetChild(song).GetComponent<player_b>().track_song;
        parnt_obj.transform.GetChild(song).GetComponent<player_b>()._do_stuff_m();
        bocina.Play();
        playstat = true;
    }

    public void _repe_song()
    {
        //bocina.loop = true;
        //playstat = !playstat;
        loop_s = !loop_s;
        if (loop_s == true)
        {
            bocina.loop = loop_s;
        }
        else if (loop_s == false)
        {
            bocina.loop = loop_s;
        }
    }

    public void _shuf_song()
    {

    }
}
