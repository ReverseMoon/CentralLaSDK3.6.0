
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MirrorTripleCent : UdonSharpBehaviour
{
    [SerializeField]
    GameObject Encender;
    [SerializeField]
    GameObject Apagar1;
    [SerializeField]
    GameObject Apagar2;
    [SerializeField]
    AudioSource Sonido;
    [SerializeField]
    GameObject Tablet;
    [SerializeField]
    Material MatEncender;
    [SerializeField]
    Material MatEstandar;




    public void _ToogleQuality()
    {
        Debug.Log("wenassss");
        Encender.SetActive(!Encender.activeSelf);
        if (Encender.activeSelf)
        {//prender color letra texto
            var mat = Tablet.GetComponent<MeshRenderer>().materials;
            mat[1] = MatEncender;
            Tablet.GetComponent<MeshRenderer>().materials = mat;
        }
        else
        {
            var mat = Tablet.GetComponent<MeshRenderer>().materials;
            mat[1] = MatEstandar;
            Tablet.GetComponent<MeshRenderer>().materials = mat;
        }
        Sonido.Play();
        Apagar1.SetActive(false);
        Apagar2.SetActive(false);
    }
    public void _OffMirror()
    {
        var mat = Tablet.GetComponent<MeshRenderer>().materials;
        mat[1] = MatEstandar;
        Tablet.GetComponent<MeshRenderer>().materials = mat;
        Encender.SetActive(false);
        Apagar1.SetActive(false);
        Apagar2.SetActive(false);
    }
}
