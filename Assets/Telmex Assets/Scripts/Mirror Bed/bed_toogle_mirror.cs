
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.SDKBase.Midi;
using VRC.Udon;

public class bed_toogle_mirror : UdonSharpBehaviour
{
    [SerializeField]
    GameObject Mirror;

    [SerializeField]
    Image BotonCanvas;

    [SerializeField]
    Color32 ColorActivado;
    [SerializeField]
    Color32 ColorApagado;

    public void Start()
    {
        BotonCanvas.color = ColorApagado;
    }
    public void _tooglebed()
    {
        Mirror.SetActive(!Mirror.activeSelf);
        if (Mirror.activeSelf == false)//Se VA A APAGAR EL MIRROR
        {
            BotonCanvas.color = ColorApagado;
        }
        else
        {
            BotonCanvas.color = ColorActivado;
        }
    }
}
