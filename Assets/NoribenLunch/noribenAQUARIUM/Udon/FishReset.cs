
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishReset : UdonSharpBehaviour
{
    public GameObject FishParticleObject;
    public ParticleSystem FishParticle;

    void Start()
    {
        
    }

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ParticleReset");
        
        
    }

    public void  ParticleReset() {
        FishParticleObject.SetActive(false);
        FishParticleObject.SetActive(true);

        FishParticle.Play();
    }


}
