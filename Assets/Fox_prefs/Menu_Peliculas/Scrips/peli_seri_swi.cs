
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class peli_seri_swi : UdonSharpBehaviour
{
    public GameObject peli_obj, seri_obj;
    bool seri = false;
    public Text txt_btn;

    public void _switch_seri() {
        seri=!seri;
        if (seri==true) {
            txt_btn.text = "Ver Peliculas";
            peli_obj.SetActive(!seri);
            seri_obj.SetActive(seri);
        }
        if (seri == false)
        {
            txt_btn.text = "Ver Series";
            peli_obj.SetActive(!seri);
            seri_obj.SetActive(seri);
        }
    }
}
