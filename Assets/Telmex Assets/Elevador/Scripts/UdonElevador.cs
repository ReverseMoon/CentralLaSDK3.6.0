
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UdonElevador : UdonSharpBehaviour
{
    public PuertaElevador puertaElevador;
    public Animator puertas;
    public Animator[] puertasPisos;
    public Transform[] pisos;
    public int tiempoDeEsperaSec = 5;
    public float velocidad = 0.2f;
    private Vector3[] positions;
    private VRCPlayerApi localPlayer;

    [UdonSynced, HideInInspector]
    public int cooldown = 0;

    [UdonSynced, HideInInspector]
    public byte queueFloor = 1;

    [UdonSynced, HideInInspector]
    public byte nextFloor = 1;

    [UdonSynced, HideInInspector]
    public byte currentFloor = 1;

    [UdonSynced, HideInInspector]
    public float time = 0.0f;
    private float timeSmooth = 0.0f;
    private bool isInitialized = false;
    void OnEnable() 
    {
        positions = new Vector3[pisos.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = pisos[i].position;
        }
    }
    void Start()
    {
        localPlayer = Networking.LocalPlayer;
    }
    void Update()
    {
        switch (DoorAction)
        {
            case 0:
            if (currentFloor != queueFloor)
            {
                DoorAction = 1;
                RequestSerialization();
            }
            break;
            case 1:
            puertaElevador._RequestSetDoor(currentFloor, false);
            DoorAction = 2;
            RequestSerialization();
            break;
            case 2:
            Wait(60, 3);
            RequestSerialization();
            break;
            case 3:
            switch (LerpAction)
            {
                case 0:
                ResetLerp(currentFloor, queueFloor, 1);
                RequestSerialization();
                break;
                case 1:
                Lerp(currentFloor, nextFloor, 2);
                RequestSerialization();
                break;
                case 2:
                ResetLerp(nextFloor, nextFloor, 0);
                DoorAction = 4;
                RequestSerialization();
                break;
            }
            break;
            case 4:
            puertaElevador._RequestSetDoor(currentFloor, true);
            DoorAction = 5;
            RequestSerialization();
            break;
            case 5:
            Wait(60, 6);
            RequestSerialization();
            break;
            case 6:
            Wait(tiempoDeEsperaSec*60, 0);
            RequestSerialization();
            break;
        }
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (!isInitialized)
        {
            transform.position = positions[currentFloor-1];
            isInitialized = true;
        }
    }
    public override void OnDeserialization()
    {

    }

    //
    // Public Functions
    //
    
    public void _Button(byte floor)
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        Networking.SetOwner(Networking.LocalPlayer, puertaElevador.gameObject);
        queueFloor = floor;
        RequestSerialization();
    }
    //
    // Private Functions
    //



    //
    // Lerp Functions
    //

    private void ResetLerp(byte current, byte next, byte action)
    {
        puertas.SetBool("Open", false);
        time = 0.0f;
        timeSmooth = 0.0f;
        cooldown = 0;
        currentFloor = current;
        nextFloor = next;
        LerpAction = action;
    }
    private void Lerp(byte current, byte next, byte action)
    { 
        if (time < 1.0000f)
        {
            time += Mathf.Abs((velocidad/(next-current))*Time.deltaTime);
            timeSmooth = Mathf.SmoothStep(0.0f, 1.0f, time);
            transform.position = Vector3.Lerp(positions[current-1], positions[next-1], timeSmooth);
        } else {
            LerpAction = action;
        }   
    }

    //
    // DoorAction Functions
    //

    private void Wait(int time, byte action)
    {
        if (cooldown < time)
        {
            cooldown++;
        } else {
            cooldown = 0;
            DoorAction = action;
        }
    }

    //
    // Enums
    //

    /// <summary>
    /// 0 = Waiting,
    /// 1 = CloseDoors,
    /// 2 = WaitDoors,
    /// 3 = Lerp,
    /// 4 = OpenDoors,
    /// 5 = WaitDoors,
    /// 6 = Cooldown,
    /// </summary>
    [UdonSynced, HideInInspector]
    public byte DoorAction = 0;

    /// <summary>
    /// 0 = StartLerp,
    /// 1 = Lerping,
    /// 2 = EndLerp,
    /// </summary>
    [UdonSynced, HideInInspector]
    public byte LerpAction = 0;
}
