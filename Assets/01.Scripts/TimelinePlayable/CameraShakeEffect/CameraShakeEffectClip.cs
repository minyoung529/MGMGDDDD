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

    public float _strength = 3.0f;
    public float _duration = 1.0f;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Extrapolation | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraShakeEffectBehaviour>.Create (graph, template);
        var clone = playable.GetBehaviour ();
        
        clone.strength = _strength;
        clone.duration = _duration;

        return playable;
    }
}
