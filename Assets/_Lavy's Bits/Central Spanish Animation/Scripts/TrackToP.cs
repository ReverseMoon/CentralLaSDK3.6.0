
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class TrackToP : UdonSharpBehaviour
{
    VRCPlayerApi lcl;
    public Transform toTrack;
    public float smoothSpeed = 8;
    public float endingTime = 5;
    public Animator anim;
    public string param;
    public GameObject audioSouce;

    public Transform worldspawn;
    public Transform elevator;

    void Start()
    {
        SendCustomEventDelayedFrames(nameof(_StartAnim), 5);
        lcl = Networking.LocalPlayer;
    }

    public void _StartAnim()
    {
        anim.SetBool(param, true);
        audioSouce.SetActive(true);

        SendCustomEventDelayedSeconds(nameof(_KillAnim), endingTime);
    }

    public void _KillAnim()
    {
        worldspawn.position = elevator.position;
        worldspawn.rotation = elevator.rotation;

        lcl.TeleportTo(worldspawn.position, worldspawn.rotation);

        Destroy(gameObject);
    }

    public override void PostLateUpdate()
    {
        toTrack.position = lcl.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;

        toTrack.rotation = Quaternion.Slerp(toTrack.rotation, lcl.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation, Time.deltaTime * smoothSpeed);
    }
}
