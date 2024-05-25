
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AdminFollow : UdonSharpBehaviour
{
    [Header("Usuario Administradores")]
    public string Usuario;
    public GameObject BoletoDorado;
    private Quaternion _rotation = new Quaternion(0, 0, 0, 0);
    private bool followadmin = false;
    private bool adminNow = false;

    // [UdonSynced]
    private VRCPlayerApi AutorizedAdmin;

    private bool AutorizedUser(VRCPlayerApi player)
    {
        bool Autorized = false;
        if (Usuario == player.displayName)
        {
            Autorized = true;
        }
        return Autorized;
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (AutorizedUser(player))
        {
            Networking.SetOwner(player, this.gameObject);
            AutorizedAdmin = player;
            BoletoDorado.SetActive(true);
            followadmin = true;
            adminNow = true;
            RequestSerialization();
        }
        //_localPlayer = Networking.GetOwner(this.gameObject);
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (AutorizedUser(player))
        {
            BoletoDorado.SetActive(false);
            followadmin = false;
            adminNow = false;
        }
    }

    public override void OnDeserialization()
    {
        if (adminNow)
        {
            BoletoDorado.SetActive(true);
            followadmin = true;
        }
        else
        {
            BoletoDorado.SetActive(false);
            followadmin = false;
        }
    }

    private void Update()
    {
        if (followadmin)
        {
            //Debug.Log("moviendooo");
            transform.SetPositionAndRotation(AutorizedAdmin.GetBonePosition(HumanBodyBones.Head), _rotation);
        }
    }
}
