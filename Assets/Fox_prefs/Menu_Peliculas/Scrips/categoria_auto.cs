
using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class categoria_auto : UdonSharpBehaviour
{
    public GameObject obj_padre,obj_padre_seri;
    //public GameObject[] a_encender;
    public bool reset_stat = false;

    public bool Accion=false, Animacion=false, Anime=false, Aventura=false, CienciaFiccion = false, Comedia = false, Drama = false, Documental = false, Fantasia = false, Musical = false, Suspenso = false, Terror = false;

    public void _do_cat()
    {
        foreach (Transform child in obj_padre.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in obj_padre_seri.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (Accion==true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Accion == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Accion == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Animacion == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Animacion == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Animacion == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Anime == true) {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Anime == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Anime == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Aventura == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Aventura == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Aventura == true) { child.gameObject.SetActive(true); }
            }
        }
        if (CienciaFiccion == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().CienciaFiccion == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().CienciaFiccion == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Comedia == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Comedia == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Comedia == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Drama == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Drama == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Drama == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Documental == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Documental == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Documental == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Fantasia == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Fantasia == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Fantasia == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Musical == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Musical == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Musical == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Suspenso == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Suspenso == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Suspenso == true) { child.gameObject.SetActive(true); }
            }
        }
        if (Terror == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                if (child.gameObject.GetComponent<More_info>().Terror == true) { child.gameObject.SetActive(true); }
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                if (child.gameObject.GetComponent<More_info_seri>().Terror == true) { child.gameObject.SetActive(true); }
            }
        }
        
        if (reset_stat == true)
        {
            foreach (Transform child in obj_padre.transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (Transform child in obj_padre_seri.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
