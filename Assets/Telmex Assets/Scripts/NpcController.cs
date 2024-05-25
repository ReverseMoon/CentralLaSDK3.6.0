
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NpcController : UdonSharpBehaviour
{

    public GameObject StaticTarget;
    public GameObject PosPlayer;
    public Animator Avatar;
    public Animator PanelPreguntas;

    public Transform posInicialPanelPreguntas;
    public Transform posFinalPanelPreguntas;
    private Vector3 posPanelPreguntasRES;


    public UdonBehaviour DisolverPanel;
    public AudioSource AudioAdmin;
    public AudioClip[] Sonidos;

    /// <summary>
    /// SONIDOS
    /// [0] Saludo 1
    /// [1] Saludo 2
    /// [2] Despedir 1
    /// [3] Despedir 2
    /// [4] Gemido 1
    /// </summary>
    VRCPlayerApi localPlayer;
    VRCPlayerApi.TrackingData headTrackingData;
    VRCPlayerApi.TrackingData LastheadTrackingData;
    private bool FollowP = false;
    private bool ReleaseP = false;
    private bool firstSaludo = true;
    private bool firstDesp = true;
    
    bool yallegue = false;
    bool yallegue2 = false;
    public float speed = 3.5f;
    public void Start()
    {
        localPlayer = Networking.LocalPlayer;
        //GURAMOS LA POSICION ORIGINAL DEL PANEL
        posPanelPreguntasRES= posInicialPanelPreguntas.gameObject.transform.position;
        //CAMBIAMOS LA POSICION DEL PANEL PARA QUE EL PLAYER NO INTERACTUE CON EL
        posInicialPanelPreguntas.gameObject.transform.position = posFinalPanelPreguntas.position;
    }

    static bool Rand()
    {
        if ((Random.Range(0, 100) % 2) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            //REGRESAMOS EL PANEL A LA POSICION ORIGINAL
            posInicialPanelPreguntas.gameObject.transform.position = posPanelPreguntasRES;
            PanelPreguntas.SetBool("AlphaCanvas", true);
            FollowP = true;
            ReleaseP = false;
            yallegue = false;
            yallegue2 = false;
            if(Rand())
            {
                AudioAdmin.clip = Sonidos[0];
            }
            else
            {
                AudioAdmin.clip = Sonidos[1];
            }
            AudioAdmin.Play();
            Avatar.SetTrigger("saludarLyp");
        }
    }


    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            //LLEVAMOS EL PANEL FUERA DE LA INTERACCION DEL PLAYER
            posInicialPanelPreguntas.gameObject.transform.position = posFinalPanelPreguntas.position;
            PanelPreguntas.SetBool("AlphaCanvas", false);
            DisolverPanel.SendCustomEvent("Disolver");
            DisolverPanel.SendCustomEvent("_DisableObjectInteract");
            if (firstDesp)
            {
                firstDesp = false;
                Avatar.SetTrigger("despedir");
            }
            if (Rand())
            {
                AudioAdmin.clip = Sonidos[2];
            }
            else
            {
                AudioAdmin.clip = Sonidos[3];
            }
            AudioAdmin.Play();
            Avatar.SetTrigger("despedirLyp");
            LastheadTrackingData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            FollowP = false;
            ReleaseP = true;
            yallegue = false;
            yallegue2 = false;
        }
    }


    private void Update()
    {
        //DEJA DE SEGUIR LA CABEZA DEL JUGADOR
        if (ReleaseP == true)
        {
            if (yallegue2)
            {
                PosPlayer.transform.position = StaticTarget.transform.position;
                PosPlayer.transform.rotation = StaticTarget.transform.rotation;
                return;
            }
            float step = speed * Time.deltaTime; // calculate distance to move
            float distance = Vector3.Distance(PosPlayer.transform.position, StaticTarget.transform.position);
            if (distance < 0.8) // at the end it will slow down to half of its speed
            {
                step = step / 2;
            }
            PosPlayer.transform.position = Vector3.MoveTowards(PosPlayer.transform.position, StaticTarget.transform.position, step);
            if (distance < 0.001f)
            {
                yallegue2 = true;
            }
        }
        //SIGUE LA CABEZA DEL JUGADOR
        if (FollowP==true)
        { // Moves the object forward at two units per second.
            headTrackingData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            if (yallegue)
            {
                PosPlayer.transform.position = headTrackingData.position;
                PosPlayer.transform.rotation = headTrackingData.rotation;
                return;
            }
            float step = speed * Time.deltaTime; // calculate distance to move
            float distance = Vector3.Distance(PosPlayer.transform.position, headTrackingData.position);
            if (distance < 0.8) // at the end it will slow down to half of its speed
            {
                step = step / 2;
            }
            PosPlayer.transform.position = Vector3.MoveTowards(PosPlayer.transform.position, headTrackingData.position, step);
            if (distance < 0.001f)
            {
                if (firstSaludo)
                {
                    firstSaludo = false;
                    Avatar.SetTrigger("saludar");
                }
                yallegue = true;
            }

        }
    }
    
}
