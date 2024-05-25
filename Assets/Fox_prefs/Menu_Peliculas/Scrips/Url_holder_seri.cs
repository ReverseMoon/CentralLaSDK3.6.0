
using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Url_holder_seri : UdonSharpBehaviour
{
    public VRCUrl[] link_hold = new VRCUrl[0];

    public Info_sys sys;

    public void _do_thingy() {
        sys.url_buton = link_hold;
        sys._put_movie();
        sys._anim_part();
    }
}
