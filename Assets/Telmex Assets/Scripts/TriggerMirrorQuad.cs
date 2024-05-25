
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TriggerMirrorQuad : UdonSharpBehaviour
{
    [SerializeField]
    GameObject Espejo;
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            Espejo.SetActive(true);
        }
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            Espejo.SetActive(false);
        }
    }
}
