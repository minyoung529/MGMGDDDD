using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using DG.Tweening;

[Serializable]
public class CameraShakeEffectClip : PlayableAsset, ITimelineClipAsset
{
    public CameraShakeEffectBehaviour template = new CameraShakeEffectBehaviour ();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Extrapolation | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraShakeEffectBehaviour>.Create (graph, template);
        var clone = playable.GetBehaviour ();
        
        return playable;
    }

}
