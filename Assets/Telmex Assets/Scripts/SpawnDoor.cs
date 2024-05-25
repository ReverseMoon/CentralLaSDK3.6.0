
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnDoor : UdonSharpBehaviour
{
    [SerializeField]
    Animator _Animacion;

    [SerializeField]
    string _NombreAnimacion;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            _Animacion.SetBool(_NombreAnimacion, true);
        }
    }
}
