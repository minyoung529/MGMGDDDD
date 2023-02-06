using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] Color paintColor;
    [SerializeField] ParticleSystem splashParticle;

    private Rigidbody rigid;
    private SphereCollider col;
    private MeshRenderer meshRender;

    private void OnEnable()
    {
        ResetBullet();
    }

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        meshRender = GetComponent<MeshRenderer>();
    }

    private void ResetBullet()
    {
        col.radius = 0.5f;
        col.isTrigger = false;
        meshRender.enabled = true;
    }

    private void SpreadOil()
    {
        if (splashParticle.isPlaying) splashParticle.Stop();

        col.isTrigger = true;
        col.radius = 1.0f;
        meshRender.enabled = false;
    }

    #region Collider


    private void OnCollisionEnter(Collision collision)
    {
        Fire[] fires = collision.collider.GetComponents<Fire>();
        if(fires.Length> 0 )
        {
            Destroy(gameObject, 0.2f);
        }
        foreach (Fire f in fires)
        {
            f.Burn();
            
        }
    }

    #endregion
}
