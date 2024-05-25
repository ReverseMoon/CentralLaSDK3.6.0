
using System.Drawing;
using UdonSharp;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]

public class player_b : UdonSharpBehaviour
{
    public AudioClip track_song;
    public int num_track;
    public AudioSource a_source;
    //public Sprite img_pau;

    //public UnityEngine.UI.Image but_play1, but_play2;
    public music_sys sys;

    public UnityEngine.UI.Image img_init;
    public UnityEngine.UI.Image[] img_dest;
    public Text tit_init, art_init;
    public Text[] tit_dest, art_dest;
    
    public void _do_stuff_m() {
        //img_dest.sprite = img_init.sprite;
        //tit_dest.text = tit_init.text;
        //art_dest.text = art_init.text;
        sys._rep_but();
        sys.song = num_track;
        sys.bocina.Stop();
        sys.bocina.clip = track_song;
        sys.bocina.Play();

        

        for (int i = 0; i < img_dest.Length; i++)
        {
            img_dest[i].sprite = img_init.sprite;
        }
        for (int i = 0; i < tit_dest.Length; i++)
        {
            tit_dest[i].text = tit_init.text;
        }
        for (int i = 0; i < art_dest.Length; i++)
        {
            art_dest[i].text = art_init.text;
        }


    }
}
