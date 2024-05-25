
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class sound_sys : UdonSharpBehaviour
{
    public AudioSource sonido;

    public void _do_sound(){
        sonido.Stop();
        sonido.Play();
    }
}
