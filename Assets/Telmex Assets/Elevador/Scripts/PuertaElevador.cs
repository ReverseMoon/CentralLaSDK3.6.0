
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class PuertaElevador : UdonSharpBehaviour
{
    public UdonElevador elevador;
    public Animator puertaPiso1;
    public Animator puertaPiso2;
    public Animator puertaPiso3;
    public Animator puertaPiso4;

    [UdonSynced, HideInInspector]
    public bool isPuertaElevadorOpen = false;
    
    [UdonSynced, HideInInspector]
    public bool isPuertaPiso1Open = false;
    
    [UdonSynced, HideInInspector]
    public bool isPuertaPiso2Open = false;
    
    [UdonSynced, HideInInspector]
    public bool isPuertaPiso3Open = false;
    
    [UdonSynced, HideInInspector]
    public bool isPuertaPiso4Open = false;
    private bool isInitialized = false;
    void Start()
    {
        
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (!isInitialized)
        {
            SetDoor(elevador.puertas, true);
            SetDoor(puertaPiso1, true);
            isPuertaElevadorOpen = true;
            isPuertaPiso1Open = true;
            isInitialized = true;
            RequestSerialization();
        }
    }
    public override void OnDeserialization()
    {
        isInitialized = true;
        CheckDoors();
    }

    //
    // Public Functions
    //
    
    public void _RequestSetDoor(byte door, bool Bool)
    {
        switch (door)
        {
            case 1:
            
            SetSync(Bool, Bool, false, false, false);
            CheckDoors();
            RequestSerialization();
            break;
            case 2:
            SetSync(Bool, false, Bool, false, false);
            CheckDoors();
            RequestSerialization();
            break;
            case 3:
            SetSync(Bool, false, false, Bool, false);
            CheckDoors();
            RequestSerialization();
            break;
            case 4:
            SetSync(Bool, false, false, false, Bool);
            CheckDoors();
            RequestSerialization();
            break;
        }
    }

    //
    // Private Functions
    //

    private void SetSync(bool bool0, bool bool1, bool bool2, bool bool3, bool bool4)
    {
        isPuertaElevadorOpen = bool0;
        isPuertaPiso1Open = bool1;
        isPuertaPiso2Open = bool2;
        isPuertaPiso3Open = bool3;
        isPuertaPiso4Open = bool4;
    }
    private void CheckDoors()
    {
        if (isPuertaElevadorOpen)
        {
            SetDoor(elevador.puertas, true);
        } else {
            SetDoor(elevador.puertas, false);
        }
        if (isPuertaPiso1Open)
        {
            SetDoor(puertaPiso1, true);
            SetDoor(puertaPiso2, false);
            SetDoor(puertaPiso3, false);
            SetDoor(puertaPiso4, false);
        } else {
            SetDoor(puertaPiso1, false);
        }
        if (isPuertaPiso2Open)
        {
            SetDoor(puertaPiso1, false);
            SetDoor(puertaPiso2, true);
            SetDoor(puertaPiso3, false);
            SetDoor(puertaPiso4, false);
        } else {
            SetDoor(puertaPiso2, false);
        }
        if (isPuertaPiso3Open)
        {
            SetDoor(puertaPiso1, false);
            SetDoor(puertaPiso2, false);
            SetDoor(puertaPiso3, true);
            SetDoor(puertaPiso4, false);
        } else {
            SetDoor(puertaPiso3, false);
        }
        if (isPuertaPiso4Open)
        {
            SetDoor(puertaPiso1, false);
            SetDoor(puertaPiso2, false);
            SetDoor(puertaPiso3, false);
            SetDoor(puertaPiso4, true);
        } else {
            SetDoor(puertaPiso4, false);
        }
    }
    private void SetDoor(Animator door, bool state)
    {
        door.SetBool("Open", state);
    }
}
