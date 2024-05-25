
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BotonElevador : UdonSharpBehaviour
{
    public UdonElevador elevador;
    public Animator animator;
    public byte piso;
    public override void Interact()
    {
        animator.SetTrigger("Boton");
        elevador._Button(piso);
    }
}
