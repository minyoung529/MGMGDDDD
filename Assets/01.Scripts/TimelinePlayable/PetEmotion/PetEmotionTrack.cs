using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.9959899f, 1f, 0f)]
[TrackClipType(typeof(PetEmotionClip))]
[TrackBindingType(typeof(PetEmotion))]
public class PetEmotionTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PetEmotionMixerBehaviour>.Create (graph, inputCount);
    }
}
