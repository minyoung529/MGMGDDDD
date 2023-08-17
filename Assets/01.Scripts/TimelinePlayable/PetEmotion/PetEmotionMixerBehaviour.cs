using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PetEmotionMixerBehaviour : PlayableBehaviour
{
    PetEmotion petEmotion;
    PetEmotionBehaviour input;

    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        petEmotion = playerData as PetEmotion;

        if (!petEmotion)  return;

        int inputCount = playable.GetInputCount();
        int currentInputCount = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<PetEmotionBehaviour> inputPlayable = (ScriptPlayable<PetEmotionBehaviour>)playable.GetInput(i);
            input = inputPlayable.GetBehaviour();

            input.petEmotion = petEmotion;

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
            if (petEmotion == null) return;
            petEmotion.SetEmotion(input.Type);
        }
        // 클립이 2개 이상인 블렌딩되고 있는 곳을 지나는 중
        else
        {
            if (petEmotion == null) return;
            petEmotion.SetEmotion(input.Type);
        }
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (petEmotion == null) return;
            petEmotion.SetEmotion(input.Type);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (petEmotion == null) return;
            petEmotion.SetEmotion(input.Type);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        if (petEmotion == null) return;
            petEmotion.SetEmotion(EmotionType.None);
    }
}
