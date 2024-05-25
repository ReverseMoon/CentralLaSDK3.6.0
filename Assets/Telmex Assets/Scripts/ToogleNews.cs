
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.Base;

public class ToogleNews : UdonSharpBehaviour
{
    [SerializeField]
    GameObject pantallaVideo;

    [SerializeField]
    GameObject panelSprite;

    BaseVRCVideoPlayer Videoplayer=null;

    public void Start()
    {
        Videoplayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer));
    }

    public void _playpauseTele()
    {
        if (Videoplayer ==null)
        {
            Videoplayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer));
        }
        if (Videoplayer.IsPlaying)
        {
            pantallaVideo.SetActive(false);
            panelSprite.SetActive(true);
            Videoplayer.Stop();
        }
        else
        {
            pantallaVideo.SetActive(true);
            panelSprite.SetActive(false);
            Videoplayer.Play();
        }
    }
}
