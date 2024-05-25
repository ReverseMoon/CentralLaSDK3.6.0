
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class OffMirror : UdonSharpBehaviour
{
    [SerializeField]
    UdonBehaviour MirrorTriple;

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            MirrorTriple.SendCustomEvent("_OffMirror");
        }
    }
}
