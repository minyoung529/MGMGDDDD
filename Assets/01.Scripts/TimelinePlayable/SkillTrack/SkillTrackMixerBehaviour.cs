using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkillTrackMixerBehaviour : PlayableBehaviour
{
    SkillVisual skill;
    
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        skill = playerData as SkillVisual;

        if (!skill) return;

        int inputCount = playable.GetInputCount();
        int currentInputCount = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<SkillTrackBehaviour> inputPlayable = (ScriptPlayable<SkillTrackBehaviour>)playable.GetInput(i);
            SkillTrackBehaviour input = inputPlayable.GetBehaviour ();

            input.skill = skill;

            // Use the above variables to process each frame of this playable.
            if (inputWeight > 0f)
                currentInputCount++;
        }

        // Ŭ���� ���� ���� ������ ��
        if (currentInputCount == 0)
        {
        }
        // Ŭ���� 1���� ���� ������ ��
        else if (currentInputCount == 1)
        {
            if (skill == null) return;
            skill.Trigger();
        }
        // Ŭ���� 2�� �̻��� �����ǰ� �ִ� ���� ������ ��
        else
        {
            if (skill == null) return;
            skill.Trigger();
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        if (skill == null) return;

        skill.Trigger();
    }
}
