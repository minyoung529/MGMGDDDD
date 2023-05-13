using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using DG.Tweening;


public class CameraShakeEffectMixerBehaviour : PlayableBehaviour
{

    float curDuration = 0f;
    Vector3 originPos = Vector3.zero;   
    CameraShakeEffectBehaviour input;

    CinemachineVirtualCameraBase activeCamera;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);

        if(activeCamera) originPos = activeCamera.transform.position;
    }

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

            input.activeCamera = activeCamera;

            if (inputWeight > 0f)
            {
                currentInputCount++;
            }
        }
        if(curDuration > 0)
        {
        //    activeCamera.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * 3f + originPos;
        }
        // Ŭ���� ���� ���� ������ ��
        if (currentInputCount == 0)
        {
        }
        // Ŭ���� 1���� ���� ������ ��
        else if (currentInputCount == 1)
        {
        }
        // Ŭ���� 2�� �̻��� �����ǰ� �ִ� ���� ������ ��
        else
        {
        }
    }


}
