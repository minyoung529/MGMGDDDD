using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PetEmotionClip : PlayableAsset, ITimelineClipAsset
{
    public PetEmotionBehaviour template = new PetEmotionBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PetEmotionBehaviour>.Create (graph, template);
        PetEmotionBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
