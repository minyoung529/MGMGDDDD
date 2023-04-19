using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionForce = 100f;

    [SerializeField]
    private float explosionRadius = 7.5f;

    [SerializeField]
    private List<ParticleSystem> explosionParticles;

    private bool isExplosioning = false;
    private Vector3 originalScale = Vector3.zero;

    [SerializeField]
    private Ease ease = Ease.InBounce;

    [SerializeField]
    private Transform visual;

    [SerializeField]
    private float upPower = 5f;

    private JumperObject jumper;

    [SerializeField]
    private List<GameObject> ignoreList;

    [SerializeField]
    private StickyPet stickyPet;

    private void Awake()
    {
        originalScale = visual.localScale;
        jumper = GetComponent<JumperObject>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isExplosioning) return;

        Fire fire = collision.gameObject.GetComponent<Fire>();

        if (fire && fire.IsBurn)
        {
            ExplosionAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isExplosioning) return;

        Fire fire = other.GetComponent<Fire>();

        if (fire && fire.IsBurn)
        {
            ExplosionAnimation();
        }
    }

    private void Explosion()
    {
        visual.DOScale(originalScale * 0.45f, 0.2f);

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

        explosionParticles.ForEach(x => x.Play());
    }

    [ContextMenu("Explosion")]
    private void ExplosionAnimation()
    {
        if (isExplosioning) return;

        isExplosioning = true;

        if (jumper)
            jumper.CanJump = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(visual.DOScale(originalScale * 6f, 1.5f).SetEase(ease));
        seq.AppendCallback(Explosion);
        seq.AppendInterval(3f);
        seq.AppendCallback(SetOriginalPos);
        seq.AppendInterval(1f);
        seq.AppendCallback(EndExplosion);
    }

    private void SetOriginalPos()
    {
        visual.DOScale(originalScale, 1f);
    }

    private void EndExplosion()
    {
        isExplosioning = false;

        if (jumper)
        {
            jumper.CanJump = true;
        }

        Debug.Log("END EXPLOSION");
        stickyPet.ChangeState(StickyState.Idle);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
