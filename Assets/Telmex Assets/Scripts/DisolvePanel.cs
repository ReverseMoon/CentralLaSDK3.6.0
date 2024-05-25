
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DisolvePanel : UdonSharpBehaviour
{
    public Animator Globo;
    public UdonBehaviour[] ControladorRespuestas;

    [SerializeField]
    private Transform posInicialGloboTexto;

    [SerializeField]
    private Transform posFinalGloboTexto;

    private Vector3 posGloboTextoRES;

    private bool contar = false;
    public float coolDown = 1f;
    private float coolDownRES;
    public void Start()
    {
        //GURAMOS LA POSICION ORIGINAL DEL PANEL
        posGloboTextoRES = posInicialGloboTexto.gameObject.transform.position;
        //CAMBIAMOS LA POSICION DEL PANEL PARA QUE EL PLAYER NO INTERACTUE CON EL
        posInicialGloboTexto.gameObject.transform.position = posFinalGloboTexto.position;
    }

    public void _DisableObjectInteract()
    {
        //LLEVAMOS EL PANEL FUERA DE LA INTERACCION DEL PLAYER
        posInicialGloboTexto.gameObject.transform.position = posFinalGloboTexto.position;
    }

    public void _EnableObjectInteract()
    {
        //REGRESAMOS EL PANEL A LA POSICION ORIGINAL
        posInicialGloboTexto.gameObject.transform.position = posGloboTextoRES;
    }
    public void Disolver()
    {
        coolDownRES = coolDown;
        Globo.SetBool("AlphaCanvas", false);
        for(int i=0;i< ControladorRespuestas.Length; i++)
        {
            ControladorRespuestas[i].SendCustomEvent("ResetGlobotTXT");
        }
        contar = true;
    }

    public void Update()
    {
        if (contar == true)
        {
            coolDown -= Time.deltaTime;
            if (coolDown <= .2f)
            {
                coolDown = coolDownRES;
                //LLEVAMOS EL PANEL FUERA DE LA INTERACCION DEL PLAYER
                posInicialGloboTexto.gameObject.transform.position = posFinalGloboTexto.position;
                contar = false;
            }
        }
    }
}
