using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using DG.Tweening;

[Serializable]
public class CameraShakeEffectBehaviour : PlayableBehaviour
{
    public CinemachineVirtualCameraBase camera;
    public float duration = 1f;
    public float strength = 2f;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        camera.transform.DOShakePosition(duration, strength);
    }
}
