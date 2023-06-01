using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionForce = 100f;

    [SerializeField]
    private float explosionRadius = 7.5f;

    private bool isExplosioning = false;

    [SerializeField]
    private float upPower = 5f;

    private JumperObject jumper;

    [SerializeField]
    private List<GameObject> ignoreList;

    [SerializeField]
    private StickyPet stickyPet;

    [SerializeField]
    private UnityEvent onExplosion;

    [SerializeField]
    private UnityEvent onBomb;

    [SerializeField]
    private ExplosionSkillVisual bombSkillVisual;

    private Fire fire;

    private void Awake()
    {
        jumper = GetComponent<JumperObject>();
        fire = GetComponent<Fire>();
        fire.OnBurn.AddListener(Explosion);

        bombSkillVisual?.ListenCompleteEvent(() => stickyPet.Event.TriggerEvent((int)PetEventName.OnExplosionEnd));
        bombSkillVisual?.ListenOnSizeSmall(() => stickyPet.Event.TriggerEvent((int)PetEventName.OnExplosionSmall));
    }

    private void DetectExplosionObject()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in cols)
        {
            if (ignoreList.Contains(col.gameObject)) continue;

            ExplosionReceiver receiver = col.GetComponent<ExplosionReceiver>();

            Rigidbody rigid = col.GetComponent<Rigidbody>();

            if (receiver)
            {
                float force = (rigid) ? rigid.mass * explosionRadius : explosionRadius;

                receiver.OnExplosion(new ExplosionInfo { explosionPos = transform.position, explosionForce = force, radius = explosionRadius });
            }

            if (rigid)
            {
                rigid.AddExplosionForce(explosionForce * rigid.mass, transform.position, explosionRadius, upPower);
            }
        }

        onBomb?.Invoke();
    }

    [ContextMenu("Explosion")]
    private void Explosion()
    {
        if (isExplosioning) return;

        stickyPet.Event.TriggerEvent((int)PetEventName.OnExplosion);
        isExplosioning = true;

        if (jumper)
            jumper.CanJump = false;

        onExplosion?.Invoke();

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.5f);
        seq.AppendCallback(DetectExplosionObject);
        seq.AppendInterval(4f);
        seq.AppendCallback(EndExplosion);
    }

    private void EndExplosion()
    {
        isExplosioning = false;

        if (jumper)
        {
            jumper.CanJump = true;
        }

        stickyPet.State.ChangeState((int)PetStateName.Idle);
        gameObject?.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void OnDestroy()
    {
        if (fire && fire.OnBurn != null)
        {
            fire.OnBurn.RemoveListener(Explosion);
        }
    }
}
