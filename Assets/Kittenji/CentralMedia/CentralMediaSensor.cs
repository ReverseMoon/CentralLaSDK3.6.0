
using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Kittenji {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), RequireComponent(typeof(BoxCollider))]
    public class CentralMediaSensor : UdonSharpBehaviour
    {
        public CentralMediaLibrary MediaLibrary;
        public USharpVideoPlayer TargetVideoPlayer;
        public bool IsOpen;
        public int TargetID; // Generated before upload
        private BoxCollider Collider;

        private void Start()
        {
            if (!Utilities.IsValid(Collider))
                Collider = GetComponent<BoxCollider>();
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            OnPlayerEnter(player, true);
        }
        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            OnPlayerEnter(player, false);
        }
        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            OnPlayerEnter(player, false);
        }

        private void OnPlayerEnter(VRCPlayerApi player, bool value)
        {
            if (Utilities.IsValid(player) && player.isLocal)
                TogglePanel(value);
        }

        private void TogglePanel(bool value)
        {
            IsOpen = value;

            if (value)
            {
                MediaLibrary.transform.position = transform.position;
                MediaLibrary.transform.rotation = transform.rotation;
                MediaLibrary.SetSensor(this);

                MediaLibrary.OpenMainPanel();
            } else
            {
                MediaLibrary.CloseMainPanel();
            }
        }
    }
}