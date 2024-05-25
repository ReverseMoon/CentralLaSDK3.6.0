
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.Base;

public class VideoController : UdonSharpBehaviour
{
    [SerializeField]
    BaseVRCVideoPlayer[] Videoplayers;

    [SerializeField]
    VRCUrl[] VideoplayerUrl;

    [SerializeField]
    float CooldownInVideos = 6f;

    [SerializeField]
    float CooldownInJoined = 15f;

    [SerializeField]
    GameObject[] Paneles;

    [SerializeField]
    Material[] VideoReal;

    float timeCooldownRES;

    private bool runplayer = false;

    int enti = 1;
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            SendCustomEventDelayedSeconds("IniciarReproducciones", CooldownInJoined);
        }
    }

    public void IniciarReproducciones()
    {
        //reproducir el primer video
        Paneles[0].GetComponent<Renderer>().material = VideoReal[0];
        Videoplayers[0].PlayURL(VideoplayerUrl[0]);
        timeCooldownRES = CooldownInVideos;
        runplayer = true;
    }

    public void Update()
    {
        if (runplayer == true)
        {
            CooldownInVideos -= Time.deltaTime;
            if (CooldownInVideos <= .2f&& enti < Videoplayers.Length)
            {
                Paneles[enti].GetComponent<Renderer>().material = VideoReal[enti];
                Videoplayers[enti].PlayURL(VideoplayerUrl[enti]);
                enti++;
                CooldownInVideos = timeCooldownRES;
            }
            if(Videoplayers.Length== enti)
            {
                runplayer = false;
            }
        }
    }
}
