using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;

public class BossSFXObject : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;
    private ParticleSystem particle;

    [SerializeField]
    private bool exclamationMark = true;

    public bool ExclamationMark => exclamationMark;

    public Action<Vector3, float, string> OnTrigger { get; set; }
    public Action<BossSFXObject> OnDestroy { get; set; }

    private void Start()
    {
        // �̸����� �����ϱ� ������ ��� �̸��� �ٸ��� ó��
        name += gameObject.GetInstanceID();
    }

    [ContextMenu("Play")]
    public void PlaySFX()
    {
        OnTrigger?.Invoke(transform.position, radius, name);
    }

    public void StartListenTrigger(Action<Vector3, float, string> action)
    {
        OnTrigger += action;
    }

    public void StopListenTrigger(Action<Vector3, float, string> action)
    {
        OnTrigger -= action;
    }

    public void StartListenDestroyed(Action<BossSFXObject> action)
    {
        OnDestroy += action;
    }

    public void StopListenDestroyed(Action<BossSFXObject> action)
    {
        OnDestroy -= action;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}