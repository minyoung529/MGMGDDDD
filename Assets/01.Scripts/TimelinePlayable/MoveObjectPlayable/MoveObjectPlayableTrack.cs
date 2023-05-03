using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.5088124f, 1f)]
[TrackClipType(typeof(MoveObjectPlayableClip))]
[TrackBindingType(typeof(Transform))]
public class MoveObjectPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MoveObjectPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
