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

    float curDuration = 0f;
    Vector3 originPos = Vector3.zero;   
    CameraShakeEffectBehaviour input;

    CinemachineVirtualCameraBase activeCamera;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);

        if(activeCamera) originPos= activeCamera.transform.position;
    }

    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        activeCamera = playerData as CinemachineVirtualCameraBase;
        if (!activeCamera) return;
        
        int inputCount = playable.GetInputCount();
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
        if(curDuration > 0)
        {
            activeCamera.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * 3f + originPos;
            curDuration -= Time.deltaTime * 0.1f;
        }
        // 클립이 없는 곳을 지나는 중
        if (currentInputCount == 0)
        {
            curDuration = 0f;
        }
        // 클립이 1개인 곳을 지나는 중
        else if (currentInputCount == 1)
        {
            if (input != null)
            {
                curDuration = duration;
            }
        }
        // 클립이 2개 이상인 블렌딩되고 있는 곳을 지나는 중
        else
        {
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        curDuration = 0f;
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        curDuration = 0f;
    }

}
