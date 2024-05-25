
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


namespace UdonSharp.Video
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Info_sys : UdonSharpBehaviour
    {
        public Animator main_screen, info_screen;

        public VRCUrl[] url_buton = new VRCUrl[0];

        public USharpVideoPlayer manager;

    public void _go_back()
        {
            _anim_part();
        }

        public void _put_movie()
        {
            manager.PlayVideo(url_buton[0]);
            _anim_part();
        }

        public void _anim_part() {
            main_screen.Play("Get_on");
            info_screen.Play("Get_off");
        }
    }
}