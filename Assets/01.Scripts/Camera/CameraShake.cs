using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private bool loop = false;

    new private Transform camera => CameraSwitcher.activeCamera.transform;

    public void Shake(float _duration)
    {
        camera.DOShakePosition(_duration);
    }
    public void Shake()
    {
        camera.DOShakePosition(duration);
    }
    public void ShakeLooping()
    {
        StartCoroutine(Shaking());
    }

    public IEnumerator Shaking()
    {
        while(loop)
        {
            yield return new WaitForSeconds(0.25f);
            camera.DOShakePosition(duration, 0.7f);
        }
    }
   
    
    public void SetLoop(bool value)
    {
        loop = value;
    }

    public void StopShake()
    {
        camera.DOKill();
    }
}
