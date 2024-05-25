
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PublicRoom : UdonSharpBehaviour
{

    [SerializeField]
    AudioSource SonidoBotonError;
    [Space(5)]

    [SerializeField]
    UdonBehaviour BtnLockforOthers;

    [SerializeField]
    UdonBehaviour ZonePlayerDetector;
    [Space(20)]
    [Tooltip("Unity UI or TMPro")]

    [Space(5)]
    [SerializeField]
    GameObject SpawnSalida;
    private bool OpenForALL = false;


    private int ContarValidos(int[] arr)
    {
        int validos = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)//SI NO ESTA VACIO EL ESPCIO DE ID
            {
                validos++;
            }
        }
        return validos;
    }

    public override void Interact()
    {
        Debug.Log("se llamaaa" + this.gameObject.name);
            if (this.gameObject.name == "BotonLobby")//SI VA ENTRANDO AL PUBLIC ROOM
            {
                OpenForALL = (bool)BtnLockforOthers.GetProgramVariable("OpenForALL");
                if (ContarValidos((int[])ZonePlayerDetector.GetProgramVariable("Public_Number_Players")) == 0)
                {
                    OpenForALL = true;//entonces abrir al que va a entrar
                }

                if (OpenForALL)
                {
                    Networking.LocalPlayer.TeleportTo(SpawnSalida.transform.position, SpawnSalida.transform.rotation);
                }
                else
                {
                    if (SonidoBotonError != null)
                    {
                        SonidoBotonError.GetComponent<AudioSource>().Play();
                    }
                }
            }
            else//SI VA SALIENDO DEL PUBLIC ROOM
            {
                Networking.LocalPlayer.TeleportTo(SpawnSalida.transform.position, SpawnSalida.transform.rotation);
            }
    }
}
