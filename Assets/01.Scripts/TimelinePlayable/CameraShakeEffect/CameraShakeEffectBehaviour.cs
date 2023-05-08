using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using DG.Tweening;

[Serializable]
public class CameraShakeEffectBehaviour : PlayableBehaviour
{
    public float _duration = 1.0f;
    public float _strength = 2.0f;

    [HideInInspector]
    public CinemachineVirtualCameraBase activeCamera;

    bool shaking = false;


    public override void OnPlayableCreate(Playable playable)
    {

    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        CameraShake();
    }

    public void CameraShake()
    {
        if (shaking) return;
        if (activeCamera == null) return;
        shaking = true;
        activeCamera.transform.DOShakePosition(_duration, _strength).OnComplete(()=> shaking = false);
    }

}
