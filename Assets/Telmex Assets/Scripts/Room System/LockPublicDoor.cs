using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI; //needs to be added to interact with unity ui components.
using VRC.SDKBase;
using VRC.Udon;
using TMPro;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class LockPublicDoor : UdonSharpBehaviour
{

    [SerializeField]
    AudioSource SonidoClic;
    [Tooltip("Unity UI or TMPro")]
    [SerializeField]
    TextMeshProUGUI TxtEstadoRoom;//LA PUERTA ESTA CERRADA
    [Space(15)]
    [SerializeField]
    Image BotonCorazon;
    [Space(15)]
    [SerializeField]
    Sprite CorazonOpen;
    [Space(15)]
    [SerializeField]
    Sprite CorazonClose;
    //TextMeshProUGUI TxtBtnRoom;//ABRIR PUERTA
    [Space(15)]
    [SerializeField]
    TextMeshProUGUI TxtEstadoLobby;//ABRIR PUERTA
    public Color32 ColorBotonVerde;
    public Color32 ColorBotonRojo;

    [UdonSynced]
    bool OpenForALL = true;

    public void Start()
    {
        BotonCorazon = GetComponent<Image>();
    }
    public void _ToogleDoorPublicSatus()
    {
            if (!Networking.IsOwner(this.gameObject))
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
           
            OpenForALL = !OpenForALL;
            if (SonidoClic != null)
            {
                SonidoClic.GetComponent<AudioSource>().Play();
            }
            if (OpenForALL == false)//Si la puerta esta Abieta para todos
            {
                BotonCorazon.sprite = CorazonOpen;
                Debug.Log("Unlock for Others");
                    //Imprimir("ABRIR<br> PUERTA", TxtBtnRoom);
                    Imprimir("La puerta esta Cerrada", TxtEstadoRoom);
                    TxtEstadoLobby.text = "CERRADO";
                    TxtEstadoLobby.color = ColorBotonRojo;
                }
                else
            {
            BotonCorazon.sprite = CorazonClose;
            Debug.Log("Lock for Others");
                //Imprimir("CERRAR<br> PUERTA", TxtBtnRoom);
                Imprimir("La puerta esta Abierta", TxtEstadoRoom);
                TxtEstadoLobby.text = "ABIERTO";
                TxtEstadoLobby.color = ColorBotonVerde;
            }
            RequestSerialization();
    }

    private void Imprimir(string Estado_IMP, TextMeshProUGUI BtnText)
    {
        if (((TextMeshProUGUI)BtnText).text != Estado_IMP)
        {
            ((TextMeshProUGUI)BtnText).text = Estado_IMP;
        }
    }
    public override void OnDeserialization()
    {
        if (OpenForALL == false)//Si la puerta esta Abieta para todos
        {
            BotonCorazon.sprite = CorazonOpen;
            //Imprimir("ABRIR<br> PUERTA", TxtBtnRoom);
            Imprimir("La puerta esta Cerrada", TxtEstadoRoom);
            TxtEstadoLobby.text = "CERRADO";
            TxtEstadoLobby.color = ColorBotonRojo;
        }
        else
        {
            BotonCorazon.sprite = CorazonClose;
            //Imprimir("CERRAR<br> PUERTA", TxtBtnRoom);
            Imprimir("La puerta esta Abierta", TxtEstadoRoom);
            TxtEstadoLobby.text = "ABIERTO";
            TxtEstadoLobby.color = ColorBotonVerde;
        }
    }

}
