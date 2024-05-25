
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SettingVideo : UdonSharpBehaviour
{
    [SerializeField]
    GameObject Panel;

    public override void Interact()
    {
        Panel.SetActive(false);
    }
}
