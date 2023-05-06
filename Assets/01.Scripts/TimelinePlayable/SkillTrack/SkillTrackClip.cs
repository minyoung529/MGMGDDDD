using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SkillTrackClip : PlayableAsset, ITimelineClipAsset
{
    public SkillTrackBehaviour template = new SkillTrackBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SkillTrackBehaviour>.Create (graph, template);
        var clone = playable.GetBehaviour ();
        return playable;
    }
}
