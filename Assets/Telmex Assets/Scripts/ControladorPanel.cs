
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ControladorPanel : UdonSharpBehaviour
{

    [SerializeField]
    GameObject ObjetoTogeable;
    [SerializeField]
    GameObject Pantalla;
    [SerializeField]
    GameObject Pantalla2;
    [SerializeField]
    Material MatEstandar;
    [SerializeField]
    Material MatEncendido;
    [SerializeField]
    Material MatEncendidoSec;
    [SerializeField]
    Material MatEncendidoDoble;
    [SerializeField]
    UdonBehaviour OtroControladorPantalla;



    //Estado espejo 
    //false==esta apaagado
    //true==esta prendido

    //mirror
    bool EstadoObjectoToogle = false;
    //collider
    bool EstadoOtroObjectoToogle = false;

    private void ChangeMaterial(GameObject pantalla,Material Asignacion)
    {
        var mat = pantalla.GetComponent<MeshRenderer>().materials;
        mat[1] = Asignacion;
        pantalla.GetComponent<MeshRenderer>().materials = mat;

    }
    public void _ToogleObjectStatus()
    {
        ObjetoTogeable.SetActive(!ObjetoTogeable.activeSelf);
        //EstadoObjectoToogle = ObjetoTogeable.activeSelf;

        if(ObjetoTogeable.name== "Espejos")
        {
            Debug.Log("nombre ESPEJOS");
            OtroControladorPantalla.SetProgramVariable("EstadoObjectoToogle", ObjetoTogeable.activeSelf);
        }
        else
        {
            Debug.Log("nombre COLLIDER");
            OtroControladorPantalla.SetProgramVariable("EstadoOtroObjectoToogle", ObjetoTogeable.activeSelf);
        }

        EstadoObjectoToogle = (bool)OtroControladorPantalla.GetProgramVariable("EstadoObjectoToogle");
        EstadoOtroObjectoToogle = (bool)OtroControladorPantalla.GetProgramVariable("EstadoOtroObjectoToogle");
        Debug.Log("EstadoObjectoToogle"+ EstadoObjectoToogle);
        Debug.Log("EstadoOtroObjectoToogle" + EstadoOtroObjectoToogle);

        
        //
        if (EstadoObjectoToogle == true)//Si el espejo esta prendido
        {
            if (EstadoOtroObjectoToogle==true)//si el collider esa prendido 
            {//ESPEJO Y COLLIDER
                Debug.Log("ESPEJO Y COLLIDER");
                ChangeMaterial(Pantalla, MatEncendidoDoble);
                ChangeMaterial(Pantalla2, MatEncendidoDoble);
            }
            else
            {//SOLO ESPEJO
                Debug.Log("SOLO ESPEJO");
                ChangeMaterial(Pantalla, MatEncendido);
                ChangeMaterial(Pantalla2, MatEncendido);
            }

        }
        else
        {
            if (EstadoOtroObjectoToogle == true)//si el collider esa prendido 
            {//SOLO COLLIDER
                Debug.Log("SOLO COLLIDER");
                ChangeMaterial(Pantalla, MatEncendidoSec);
                ChangeMaterial(Pantalla2, MatEncendidoSec);
            }
            else
            {//APAGAR TODO
                Debug.Log("APAGAR TODO");
                ChangeMaterial(Pantalla, MatEstandar);
                ChangeMaterial(Pantalla2, MatEstandar);
            }
        }

    }
  
}
