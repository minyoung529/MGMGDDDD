using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BushObject : MonoBehaviour
{
    [SerializeField] private GameObject[] bushes;
    [SerializeField] private ParticleSystem[] fires;


    [SerializeField] new private Light light;
    new private Collider collider;

    private bool isBurning = false;
    public bool IsBurn { get { return isBurning; } }

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(Define.FIRE_PET_TAG))
        {
            if (isBurning) return;
            StartCoroutine(Burn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            if (isBurning) return;
            StartCoroutine(Burn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireBall"))
        {
            if (isBurning) return;
            StartCoroutine(Burn());
        }
    }

    private IEnumerator Burn()
    {
        isBurning = true;

        light.gameObject.SetActive(true);
        float intensity = light.intensity;
        light.intensity = 0f;
        light.DOIntensity(intensity, 1f);

        for (int i = 0; i < fires.Length; i++)
        {
            if (fires[i])
            {
                fires[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            }
        }

        for (int i = bushes.Length - 1; i >= 0; i--)
        {
            bushes[i].gameObject.SetActive(false);

            if (fires[i])
                fires[i].Stop();
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }

        light.DOIntensity(0f, 0.5f).OnComplete(() => light.gameObject.SetActive(false));
        isBurning = false;

        collider.enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
