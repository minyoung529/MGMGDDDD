using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

[TrackColor(0.4528302f, 0.4528302f, 0.4528302f)]
[TrackClipType(typeof(CameraShakeEffectClip))]
[TrackBindingType(typeof(CinemachineVirtualCameraBase))]
public class CameraShakeEffectTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<CameraShakeEffectMixerBehaviour>.Create (graph, inputCount);
    }
}
