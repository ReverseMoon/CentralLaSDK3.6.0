
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;
using VRC.Udon;

public class bedmirrorv2 : UdonSharpBehaviour
{
    [SerializeField]
    AudioSource SonidoClic;

    [SerializeField]
    GameObject Mirror;

    [Space(15)]
    [SerializeField]
    Image BotonSwitch;

    [Space(15)]
    [SerializeField]
    Sprite SwitchOn;

    [Space(15)]
    [SerializeField]
    Sprite SwitchOff;

    public void Start()
    {
        BotonSwitch.sprite = SwitchOff;
    }

    public void _tooglebed()
    {
        if (SonidoClic != null)
        {
            SonidoClic.GetComponent<AudioSource>().Play();
        }
        Mirror.SetActive(!Mirror.activeSelf);
        if (Mirror.activeSelf == false)//Se VA A APAGAR EL MIRROR
        {
            BotonSwitch.sprite = SwitchOff;
        }
        else
        {
            BotonSwitch.sprite = SwitchOn;
        }
    }
}
