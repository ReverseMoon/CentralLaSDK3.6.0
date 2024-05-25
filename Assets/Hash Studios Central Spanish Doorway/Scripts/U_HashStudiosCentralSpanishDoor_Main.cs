
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class U_HashStudiosCentralSpanishDoor_Main : UdonSharpBehaviour
{
    public string roomName;
    public Camera camera1;
    public Image camera1_uiImage;
    public Image camera1_uiImage_outdoor;
    public Material camera1_material;
    public Camera camera2;
    public Image camera2_uiImage;
    public Image camera2_uiImage_outdoor;
    public Material camera2_material;
    public bool isVIP;
    public string[] vipMembers;
}
