
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TriggerAnimBool : UdonSharpBehaviour
{
    [SerializeField]
    Animator _Animacion;

    [SerializeField]
    string _NombreAnimacion;


    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            _Animacion.SetBool(_NombreAnimacion, true);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            _Animacion.SetBool(_NombreAnimacion, false);

        }
    }
}
