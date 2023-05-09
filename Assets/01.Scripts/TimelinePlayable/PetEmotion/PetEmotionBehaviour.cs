using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PetEmotionBehaviour : PlayableBehaviour
{
    [SerializeField] private EmotionType emotionType;
    public EmotionType Type { get { return emotionType; } set { emotionType = value; } }

    [HideInInspector] public Pet pet;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        EmotionSetting(emotionType);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);
        EmotionSetting(EmotionType.None);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        EmotionSetting(EmotionType.None);
    }

    private void EmotionSetting(EmotionType type)
    {
        if (pet == null) return;
        pet.Emotion.SetEmotion(type);
    }

}
