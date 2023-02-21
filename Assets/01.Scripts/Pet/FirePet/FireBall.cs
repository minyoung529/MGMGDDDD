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

        Debug.Log("Fire");
        transform.SetParent(fire.transform);
        fire.Burn();
    }


    #region Collider


    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag(Define.FIRE_PET_TAG) || other.CompareTag(Define.PLAYER_TAG) || other.CompareTag(Define.OIL_PET_TAG) ) return;

        Fire[] fires = other.GetComponents<Fire>();
        if (fires.Length > 0)
        {
            Burning(fires[0]);
            return;
        }

        IceMelting[] ices = other.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }

        Destroy(gameObject, 0.1f);
    }


    #endregion
} 
