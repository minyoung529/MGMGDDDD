using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.8679245f, 0.8679245f, 0.8679245f)]
[TrackClipType(typeof(SkillTrackClip))]
[TrackBindingType(typeof(Pet))]
public class SkillTrackTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SkillTrackMixerBehaviour>.Create (graph, inputCount);
    }
}
