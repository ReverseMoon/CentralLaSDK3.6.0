
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class MuteZonePlace : UdonSharpBehaviour
{

    [Header("Audio del Usuario ")]
    [Range(0, 24)]
    public float GananciaAudio = 0; //0 - 24
    public float DistanciaMaximaAudio = 0;
    public float DistanciaMinimaAudio = 0;
    public float RadioVolumetrico = 0;
    public bool FiltroDeVoz = false;

    [Header("Audio del Avatar de Usuario")]
    [Range(0, 10)]
    public float avatarMaxAudioGain = 0;   //0 - 10
    public float avatarMaxFarRadius = 0;
    public float avatarMaxNearRadius = 0;
    public float avatarMaxVolumetricRadius = 0;
    public bool avatarForceSpacialization = false;
    public bool avatarDisableCustomCurve = false;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        SetDefaultAudioSettings(player);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
            //Player voice
            player.SetVoiceGain(GananciaAudio);
            player.SetVoiceDistanceFar(DistanciaMaximaAudio);
            player.SetVoiceDistanceNear(DistanciaMinimaAudio);
            player.SetVoiceVolumetricRadius(RadioVolumetrico);
            player.SetVoiceLowpass(FiltroDeVoz);

            //Avatar audio
            player.SetAvatarAudioGain(avatarMaxAudioGain);
            player.SetAvatarAudioFarRadius(avatarMaxFarRadius);
            player.SetAvatarAudioNearRadius(avatarMaxNearRadius);
            player.SetAvatarAudioVolumetricRadius(avatarMaxVolumetricRadius);
            player.SetAvatarAudioForceSpatial(avatarForceSpacialization);
            player.SetAvatarAudioCustomCurve(!avatarDisableCustomCurve);
      
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        SetDefaultAudioSettings(player);
      
    }

    private void SetDefaultAudioSettings(VRCPlayerApi player)
    {
        //Player Voice
        player.SetVoiceGain(15f);
        player.SetVoiceDistanceFar(25f);
        player.SetVoiceDistanceNear(0);
        player.SetVoiceVolumetricRadius(0);
        player.SetVoiceLowpass(false);

        //Avatar audio
        player.SetAvatarAudioGain(10f);
        player.SetAvatarAudioFarRadius(40f);
        player.SetAvatarAudioNearRadius(40f);
        player.SetAvatarAudioVolumetricRadius(40f);
        player.SetAvatarAudioForceSpatial(false);
        player.SetAvatarAudioCustomCurve(false);
    }

   /* private void SetDefaultAudioSettings(VRCPlayerApi player)
    {
        //Player Voice
        player.SetVoiceGain(8);
        player.SetVoiceDistanceFar(10);
        player.SetVoiceDistanceNear(0);
        player.SetVoiceVolumetricRadius(0);
        player.SetVoiceLowpass(true);

        //Avatar audio
        player.SetAvatarAudioGain(2);
        player.SetAvatarAudioFarRadius(10);
        player.SetAvatarAudioNearRadius(0);
        player.SetAvatarAudioVolumetricRadius(0);
        player.SetAvatarAudioForceSpatial(false);
        player.SetAvatarAudioCustomCurve(false);
    }
*/
}
