using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
        meshRender.enabled = true;
    }

    private void Burning(Fire fire)
    {
        meshRender.enabled = false;

        transform.SetParent(fire.transform);
        fire.Burn();
    }


    #region Collider


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Fire")) return;

        Fire[] fires = collision.collider.GetComponents<Fire>();
        if (fires.Length > 0)
        {
            Burning(fires[0]);
            return;
        }
        IceMelting[] ices = collision.collider.GetComponents<IceMelting>();
        foreach(IceMelting ice in ices)
        {
            ice.IceMelt();
        }

        Destroy(gameObject, 0.1f);
    }



    #endregion
}
