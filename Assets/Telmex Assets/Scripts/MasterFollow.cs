
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MasterFollow : UdonSharpBehaviour
{
    private VRCPlayerApi _localPlayer;
    private Quaternion _rotation = new Quaternion(0, 0, 0,0);

    public void Start()
    {
        _localPlayer = Networking.GetOwner(gameObject);
        SendCustomEventDelayedFrames("_do_cycle", 5);
    }
    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        _localPlayer = Networking.GetOwner(this.gameObject);
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        _localPlayer = Networking.GetOwner(this.gameObject);
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        _localPlayer = Networking.GetOwner(this.gameObject);
    }
    /*private void Update()
    {
        //transform.SetPositionAndRotation(_localPlayer.GetBonePosition(HumanBodyBones.Head), _rotation);
    }*/

    public void _do_cycle() {
        transform.SetPositionAndRotation(_localPlayer.GetBonePosition(HumanBodyBones.Head), _rotation);
        SendCustomEventDelayedFrames("_do_cycle", 5);
    }
}
