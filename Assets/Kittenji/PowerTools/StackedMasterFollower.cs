
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Kittenji.Tools
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class StackedMasterFollower : UdonSharpBehaviour
    {
        [Header("Configuration")]
        public Vector3 FollowOffset;

        [Header("Basic Follower")]
        public Transform MasterTag;

        [Header("Administrator")]
        public Transform AdminTag;
        public string[] AdminList = new string[] { "ITACHIJJ", "Golis22" }; // , Kittenji... Ok no XD

        // Private Fields
        private VRCPlayerApi RoomMaster;

        private Transform[] AdminTagPool;
        private VRCPlayerApi[] AuthenticatedUsers;
        private bool AdminJoined, CanFollowAdmin, CanFollowMaster;
        VRCPlayerApi LocalPlayer;

        private bool Initialized;

        private void Start()
        {
            if (!Initialized)
            {
                LocalPlayer = Networking.LocalPlayer;
                if (AdminList != null && AdminList.Length > 0 && Utilities.IsValid(AdminTag))
                {
                    // Instantiate admin followers pool
                    GameObject prefab = AdminTag.gameObject;
                    prefab.SetActive(false);

                    int length = AdminList.Length;
                    AdminTagPool = new Transform[length];
                    AuthenticatedUsers = new VRCPlayerApi[length];
                    AdminTagPool[0] = AdminTag;
                    for (int i = 1; i < length; i++)
                    {
                        GameObject go = Instantiate(prefab);
                        if (AdminTag.parent != null) go.transform.parent = AdminTag.parent;
                        AdminTagPool[i] = go.transform;
                    }

                    CanFollowAdmin = true;
                    Debug.Log("Can follow admin.");
                }

                CanFollowMaster = Utilities.IsValid(MasterTag);

                Initialized = true;
            }

            if (LocalPlayer.isMaster)
                RoomMaster = LocalPlayer;
            else RoomMaster = Networking.GetOwner(gameObject);
        }

        private void Authenticate(VRCPlayerApi player, bool remove = false)
        {
            AdminJoined = false;
            for (int i = 0; i < AdminList.Length; i++)
            {
                string admin = AdminList[i];
                if (admin == player.displayName)
                {
                    AuthenticatedUsers[i] = remove ? null : player;
                    AdminTagPool[i].gameObject.SetActive(!remove);
                }

                AdminJoined = AdminJoined || Utilities.IsValid(AuthenticatedUsers[i]);
            }
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (player.isMaster)
            {
                RoomMaster = player;
            }
            else if (LocalPlayer.isMaster)
            {
                // Someone is trying to take ownership of this object, but they are not the master
                Networking.SetOwner(LocalPlayer, gameObject);
                RoomMaster = LocalPlayer;
            }
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player))
            {
                if (CanFollowAdmin)
                    Authenticate(player, true);
                if (player.isMaster && LocalPlayer.isMaster && !Networking.IsOwner(gameObject))
                    Networking.SetOwner(LocalPlayer, gameObject);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) && CanFollowAdmin)
            {
                Authenticate(player);
            }
        }

        public override void PostLateUpdate()
        {
            if (CanFollowMaster && Utilities.IsValid(RoomMaster))
            {
                MasterTag.position = RoomMaster.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            }

            if (CanFollowAdmin && AdminJoined)
            {
                for (int i = 0; i < AdminList.Length; i++)
                {
                    Transform tr = AdminTagPool[i];
                    VRCPlayerApi user = AuthenticatedUsers[i];
                    if (Utilities.IsValid(user))
                        tr.position = user.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position + FollowOffset;
                }
            }
        }
    }
}