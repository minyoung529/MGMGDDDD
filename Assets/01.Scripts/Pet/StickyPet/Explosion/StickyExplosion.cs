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

    private void Start()
    {
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
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in cols)
        {
            if (col.gameObject == gameObject) continue;

            ExplosionReceiver receiver = col.GetComponent<ExplosionReceiver>();

            if (receiver)
            {
                receiver.OnExplosion();
            }

            Rigidbody rigid = col.GetComponent<Rigidbody>();

            if (rigid)
            {
                rigid.AddExplosionForce(explosionForce * rigid.mass, transform.position, explosionRadius, upPower);
            }
        }

        explosionParticles.ForEach(x => x.Play());

        visual.DOScale(originalScale, 1f).OnComplete(() =>
        {
            isExplosioning = false;
            if (jumper)
                jumper.CanJump = true;

        });
    }

    [ContextMenu("Explosion")]
    private void ExplosionAnimation()
    {
        if (isExplosioning) return;

        originalScale = visual.localScale;

        isExplosioning = true;

        if (jumper)
            jumper.CanJump = false;

        visual.DOScale(originalScale * 1.75f, 1.5f).SetEase(ease).OnComplete(Explosion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
