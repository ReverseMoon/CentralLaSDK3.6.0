
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerElevador : UdonSharpBehaviour
{
    public UdonElevador elevador;
    private VRCPlayerApi localPlayer;
    private VRCPlayerApi colliderPlayer;
    private Vector3 position;
    private Quaternion rotation;
    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        colliderPlayer = null;
    }
    void LateUpdate()
    {
        if (elevador.LerpAction == 1)
        {
            Teleport(colliderPlayer);
        }
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (localPlayer == player)
        {
            colliderPlayer = player;
        }
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (localPlayer == player)
        {
            colliderPlayer = null;
        }
    }
    private void Teleport(VRCPlayerApi player)
    {
        if (player != null)
        {
            position = player.GetPosition();
            position.y = transform.position.y;
            rotation = player.GetRotation();
            player.TeleportTo(position, rotation);
        }
    }
}
