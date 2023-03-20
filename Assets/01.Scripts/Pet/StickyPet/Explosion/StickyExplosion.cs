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

    private void Start()
    {
        originalScale = visual.localScale;
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
                rigid.AddExplosionForce(explosionForce * rigid.mass, transform.position, explosionRadius, 10f);
            }
        }

        explosionParticles.ForEach(x => x.Play());
        isExplosioning = false;

        visual.DOScale(originalScale, 1f);
    }

    [ContextMenu("Explosion")]
    private void ExplosionAnimation()
    {
        isExplosioning = true;
        visual.DOScale(originalScale * 1.75f, 1.5f).SetEase(ease).OnComplete(Explosion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
