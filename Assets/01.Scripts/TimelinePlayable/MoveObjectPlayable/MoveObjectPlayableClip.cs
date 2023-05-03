using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MoveObjectPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public MoveObjectPlayableBehaviour template = new MoveObjectPlayableBehaviour ();
    public Transform endPosition;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveObjectPlayableBehaviour>.Create (graph, template);
        MoveObjectPlayableBehaviour clone = playable.GetBehaviour ();

        Debug.Log(owner.name);
       // owner.transform.DOMove(endPosition.position, 1f);

        return playable;
    }
}
