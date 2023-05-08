using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using DG.Tweening;


public class CameraShakeEffectMixerBehaviour : PlayableBehaviour
{
    float duration = 1f;
    float strength = 2f;
        CameraShakeEffectBehaviour input;

    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        CinemachineVirtualCameraBase activeCamera = playerData as CinemachineVirtualCameraBase;
        if (!activeCamera) return;
        
        int inputCount = playable.GetInputCount ();
        int currentInputCount = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<CameraShakeEffectBehaviour> inputPlayable = (ScriptPlayable<CameraShakeEffectBehaviour>)playable.GetInput(i);
            input = inputPlayable.GetBehaviour();

            input._duration = duration;
            input._strength = strength;
            input.activeCamera = activeCamera;

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
            if(input != null) input.CameraShake();
        }
        // 클립이 2개 이상인 블렌딩되고 있는 곳을 지나는 중
        else
        {
        }
    }
}
