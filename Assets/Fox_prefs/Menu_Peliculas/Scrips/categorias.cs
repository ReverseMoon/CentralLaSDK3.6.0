
using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class categorias : UdonSharpBehaviour
{
    public GameObject obj_padre;
    public GameObject[] a_encender;
    public bool reset_stat = false;


    public void _do_cat(){
        foreach (Transform child in obj_padre.transform) {
            child.gameObject.SetActive(false);
        }

        //obj_padre.transform.GetChild(song).GetComponent<player_b>()._do_stuff_m();

        if (reset_stat) {
            foreach (Transform child in obj_padre.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        if (reset_stat == false) {
            for (int i = 0; i < a_encender.Length; i++) {
                a_encender[i].SetActive(true);
            } 
        }
    }
}
