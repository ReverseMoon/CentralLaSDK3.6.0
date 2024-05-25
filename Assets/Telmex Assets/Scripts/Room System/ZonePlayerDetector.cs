
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI; //needs to be added to interact with unity ui components.
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]

public class ZonePlayerDetector : UdonSharpBehaviour
{

    [SerializeField]
    TextMeshProUGUI TextPantallaStatusLobby;
    public Color32 ColorBotonVerde;
    [Space(5)]
    [UdonSynced]
    private int[] Public_Number_Players = new int[80];

    private VRCPlayerApi _playerLOCAL;

    [SerializeField]
    TextMeshProUGUI CantidadPersonas;


    [SerializeField]
    TextMeshProUGUI TxtUsuarios;




    [Tooltip("Unity UI or TMPro")]
    [SerializeField]
    TextMeshProUGUI TxtEstadoRoom;//LA PUERTA ESTA CERRADA
    [Space(15)]
    [SerializeField]
    Image SwitchEstadoPuerta;
    [Space(15)]
    [SerializeField]
    Sprite SwitchOff;
    [SerializeField]
    UdonBehaviour LockUnlockUI;



    public void Start()
    {
        _playerLOCAL = Networking.LocalPlayer;
    }

    private void ChangeStatusRoom()
    {
        if (ContarValidos(Public_Number_Players) == 0)//SI NO HAY GENTE EN LA HABITACION
        {
            TextPantallaStatusLobby.text = "ABIERTO";
            TextPantallaStatusLobby.color = ColorBotonVerde;
            TxtEstadoRoom.text = "La puerta esta Abierta";
            SwitchEstadoPuerta.sprite = SwitchOff;
            if (Networking.IsOwner(LockUnlockUI.gameObject)){
                LockUnlockUI.SetProgramVariable("OpenForALL", true);
            }
        }
    }
    private int ContarValidos(int[] arr)
    {
        int validos = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)//SI NO ESTA VACIO EL ESPCIO DE ID
            {
                validos = validos + 1;
            }
        }
        return validos;
    }
    private int[] AddPlayer(int[] arr, int playerId)
    {
        bool band = false;
        for (int i = 0; i < arr.Length && band == false; i++)
        {
            if (arr[i] == 0)//SI ESTA VACIO EL ESPACIO
            {
                arr[i] = playerId;
                band = true;
            }
        }
        return arr;
    }
    private int[] RemovePlayer(int[] arr, int playerid)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == playerid)
            {
                arr[i] = 0;
            }
        }
        return arr;
    }

    public void ActualizarOcupantes()
    {
        CantidadPersonas.text = ContarValidos(Public_Number_Players).ToString();
        switch (ContarValidos(Public_Number_Players))
        {
            case 1:
                TxtUsuarios.text = "Usuario";
                break;
            default:
                TxtUsuarios.text = "Usuarios";
                break;
        }
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            Debug.Log("===========TRIGGER ENTER==================");
            if (!Networking.IsOwner(this.gameObject))
                Networking.SetOwner(_playerLOCAL, this.gameObject);
            Public_Number_Players = AddPlayer(Public_Number_Players, _playerLOCAL.playerId);
            ChangeStatusRoom();
            RequestSerialization();
        }
        ActualizarOcupantes();


    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            Debug.Log("===========TRIGGER EXIT==================");
            if (!Networking.IsOwner(this.gameObject))
                Networking.SetOwner(_playerLOCAL, this.gameObject);
            Public_Number_Players = RemovePlayer(Public_Number_Players, _playerLOCAL.playerId);
            ChangeStatusRoom();
            RequestSerialization();
        }
        ActualizarOcupantes();
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (_playerLOCAL.IsOwner(this.gameObject))
        {
            if (!Networking.IsOwner(this.gameObject))
                Networking.SetOwner(_playerLOCAL, this.gameObject);
            Debug.Log("===========PLAYER LEFT CALCULATE==================");
            Public_Number_Players = RemovePlayer(Public_Number_Players, player.playerId);
            ChangeStatusRoom();
            RequestSerialization();
        }
        if (Networking.IsOwner(this.gameObject))
        {
            Debug.Log("NETWKRING IS OWNER PLYAER LEFT");
        }
        ActualizarOcupantes();
    }

    public override void OnDeserialization()
    {
        Debug.Log("===========DESERIALIZAION CALCULATE PUBLIC V2==================");
        ChangeStatusRoom();
    }

    public override void OnPostSerialization(SerializationResult result)
    {
        if (!result.success)
        {
            RequestSerialization();
        }
    }
}
