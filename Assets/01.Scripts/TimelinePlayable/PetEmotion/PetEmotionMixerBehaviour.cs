using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PetEmotionMixerBehaviour : PlayableBehaviour
{
    Pet pet;
    PetEmotionBehaviour input;

    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        pet = playerData as Pet;

        if (!pet)
            return;

        int inputCount = playable.GetInputCount();
        int currentInputCount = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<PetEmotionBehaviour> inputPlayable = (ScriptPlayable<PetEmotionBehaviour>)playable.GetInput(i);
            input = inputPlayable.GetBehaviour();

            input.pet = pet;

            // Use the above variables to process each frame of this playable.
            if (inputWeight > 0f)
            {
                currentInputCount++;
            }
        }
        // 클립이 없는 곳을 지나는 중
        if (currentInputCount == 0)
        {
        }
        // 클립이 1개인 곳을 지나는 중
        else if (currentInputCount == 1)
        {

        }
        // 클립이 2개 이상인 블렌딩되고 있는 곳을 지나는 중
        else
        {
        }
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (pet == null) return;
            pet.Emotion.SetEmotion(input.Type);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (pet == null) return;
            pet.Emotion.SetEmotion(input.Type);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        if (pet == null) return;
            pet.Emotion.SetEmotion(EmotionType.None);
    }
}
