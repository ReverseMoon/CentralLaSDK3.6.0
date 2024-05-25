
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSharp.Video
{
    [AddComponentMenu("Udon Sharp/Video/UI/Sync Mode Controller")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class SyncModeController : UdonSharpBehaviour
    {
        public VideoControlHandler videoPlayerControls;

        public RectTransform sliderTransform;

        Animator _animator;
        public Text sliderText;

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        private void Start()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (sliderText == null)
                sliderText = sliderTransform.GetComponentInChildren<Text>();
        }

        public void SetVideoVisual()
        {
            _animator.SetInteger("Target", 0);
            sliderText.text = "Video";
        }

        public void SetStreamVisual()
        {
            _animator.SetInteger("Target", 1);
            sliderText.text = "Stream";
        }

        public void ClickVideoToggle()
        {
            videoPlayerControls.OnVideoPlayerModeButtonPressed();
        }

        public void ClickStreamToggle()
        {
            videoPlayerControls.OnStreamPlayerModeButtonPressed();
        }
    }
}
