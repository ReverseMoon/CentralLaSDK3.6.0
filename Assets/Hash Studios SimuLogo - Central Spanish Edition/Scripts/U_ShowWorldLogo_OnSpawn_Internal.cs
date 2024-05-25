
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.Video;
using VRC.SDK3.Video.Components.Base;
using VRC.SDK3.Components;

namespace HashStudiosSimuLogo.Scripts
{

    /// <summary>
    /// Copyright (c) 2023 Hash Studios LLC. All rights reserved.
    /// This code is the sole property of Hash Studios LLC and is protected by copyright laws.
    /// It may not be used, reproduced, or distributed without the express written permission of Hash Studios LLC.
    ///
    /// Any unauthorized use of this code is strictly prohibited and may result in legal action.
    ///
    /// This copyright notice must be included in any copies or reproductions of this code.
    ///
    /// NOTE: This copyright notice does not apply to any dependencies or
    /// external libraries used in this code, such as UdonSharp, VRC SDK, or VRChat.
    /// The copyright and licensing terms of these dependencies apply to their respective code.
    /// Hash Studios LLC is not claiming any rights to these dependencies.
    /// </summary>

    public class U_ShowWorldLogo_OnSpawn_Internal : UdonSharpBehaviour
    {

        public U_ShowWorldLogo_OnSpawn_Main mainScript;
        public Image[] WorldLogoTexture;
        public Image[] BackgroundWorldLogoTexture;
        public TextMeshProUGUI [] category1_Text;
        public TextMeshProUGUI [] category2_Text;
        public TextMeshProUGUI [] category3_Text;
        public GameObject VR_Canvas;
        public GameObject PC_Canvas;
        public Animator PC_canvasAnimator;
        public Animator VR_canvasAnimator;
        public bool followPlayerHead = false;
        public BaseVRCVideoPlayer videoPlayer;

        //public VideoPlayer videoPlayer;

        void Start()
        {
            //foreach(TextMeshProUGUI temp in category1_Text)
            //{
            //    temp.text = "<b>" + mainScript.Category1_Label + "</b>: " + mainScript.Category1_Text;
            //}
            //foreach(TextMeshProUGUI temp in category2_Text)
            //{
            //    temp.text = "<b>" + mainScript.Category2_Label + "</b>: " + mainScript.Category2_Text;
            //}
            //foreach(TextMeshProUGUI temp in category3_Text)
            //{
            //    temp.text = "<b>" + mainScript.Category3_Label + "</b>: " + mainScript.Category3_Text;
            //}
            //foreach (Image img in WorldLogoTexture)
            //{
            //    img.sprite = mainScript.WorldLogo;
            //}
            //foreach (Image img in BackgroundWorldLogoTexture)
            //{
            //    img.sprite = mainScript.Background;
            //}
            if (mainScript.timeToDisplayAfterSpawn < 0)
            {
                mainScript.timeToDisplayAfterSpawn = 0;
            }
            if (mainScript.timeToDisplayLogo <= 0.25f)
            {
                mainScript.timeToDisplayLogo = 0.25f;
            }
            //SendCustomEventDelayedSeconds("showCanvas", mainScript.timeToDisplayAfterSpawn);
            showCanvas();
        }

        void Update()
        {
            if(followPlayerHead == true)
            {
                /*Quaternion headRot = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

                Vector3 pos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;

                VR_Canvas.transform.SetPositionAndRotation(pos, headRot);

                Quaternion rot = Networking.LocalPlayer.GetRotation();

                Vector3 playerHeight = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head) - ((Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftFoot) + Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightFoot)) / 2);

                float scaleOfCanvas = (playerHeight.y / 1.6f) * 0.003f;

                VR_Canvas.transform.localScale = new Vector3(scaleOfCanvas, scaleOfCanvas, scaleOfCanvas);

                VR_Canvas.transform.SetPositionAndRotation(pos, rot);

                VR_Canvas.transform.position = pos + VR_Canvas.transform.forward * (1f * (playerHeight.y / 1.6f));

                VR_Canvas.transform.LookAt(pos);
                VR_Canvas.transform.Rotate(0, 180, 0);*/

                // Fetch tracking data for the head
                VRCPlayerApi.TrackingData headTrackingData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                Quaternion headRotation = headTrackingData.rotation;
                Vector3 headPosition = headTrackingData.position;

                // Calculate scale based on player height
                Vector3 headBonePosition = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
                Vector3 footAveragePosition = (Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftFoot) + Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightFoot)) / 2;
                float playerHeight = (headBonePosition - footAveragePosition).y;
                float scaleOfCanvas = (playerHeight / 1.6f) * 0.003f;

                // Set canvas transformation
                VR_Canvas.transform.position = headPosition; // Position the canvas at the head position
                VR_Canvas.transform.rotation = headRotation; // Rotate canvas to match head's rotation in all axes
                VR_Canvas.transform.localScale = new Vector3(scaleOfCanvas, scaleOfCanvas, scaleOfCanvas); // Scale the canvas based on player height

                // Adjust position to be in front of the player's view
                VR_Canvas.transform.position += VR_Canvas.transform.forward * (1f * (playerHeight / 1.6f)); // Position canvas in front of the head

            }
        }

        public void showCanvas()
        {
            Networking.LocalPlayer.Immobilize(true);
            if (Networking.LocalPlayer.IsUserInVR())
            {
                followPlayerHead = true;
                VR_canvasAnimator.Play("fadeIn", 0, 0f);
                //VR_canvasAnimator.Play("showCanvas", 0, 0f);
                SendCustomEventDelayedSeconds(nameof(playVideo), 2f);
            }
            else
            {
                PC_canvasAnimator.Play("fadeIn", 0, 0f);
                //PC_canvasAnimator.Play("showCanvas", 0, 0f);
                SendCustomEventDelayedSeconds(nameof(playVideo), 2f);
            }
            SendCustomEventDelayedSeconds("hideCanvas", mainScript.timeToDisplayLogo + 0.6f + 2f);
        }

        public void playVideo(){
            videoPlayer.Play();
        }

        public void hideCanvas()
        {
            Networking.LocalPlayer.Immobilize(false);
            if (Networking.LocalPlayer.IsUserInVR())
            {
                VR_canvasAnimator.Play("fadeOut", 0, 0f);
                SendCustomEventDelayedSeconds("disableCanvas", 0.6f);
            }
            else
            {
                PC_canvasAnimator.Play("fadeOut", 0, 0f);
                SendCustomEventDelayedSeconds("disableCanvas", 0.6f);
            }
        }

        public void disableCanvas()
        {
            followPlayerHead = false;
            PC_Canvas.SetActive(false);
            VR_Canvas.SetActive(false);
        }
    }
}
