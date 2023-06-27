using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuzeScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onComplete;
    [SerializeField] private UnityEvent onRecycle;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    private Material material;
    private float amount = 0;
    private float distance = 0;
    private float fireSpeed = 3f;
    private bool canFire = true;

    [SerializeField]
    private Fire fire;

    private readonly int Fill = Shader.PropertyToID("_Fill");
    private PlaySound fuzeSound;

    private void Awake()
    {
        fuzeSound = GetComponent<PlaySound>(); 
        material = GetComponent<Renderer>().material;
        distance = Vector3.Distance(startPos.position, endPos.position);

        onComplete.AddListener(() => fuzeSound.Stop());
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        Fire fire = collision.collider.GetComponent<Fire>();
        if(fire != null)
        {
            if (fire.IsBurn)
            {
                Fire();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        Fire fire = other.GetComponent<Fire>();
        if (fire != null)
        {
            if (fire.IsBurn)
            {
                Fire();
            }
        }
    }

    [ContextMenu("Trigger")]
    public void Fire()
    {
        if (!canFire) return;
        canFire = false;
        fire.gameObject.SetActive(true);
        fuzeSound.Play();
        StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        fire.BurningTime = fireSpeed * distance + 0.2f;

        while (amount < 1)
        {
            fire.Burn();
            amount += fireSpeed * Time.deltaTime / distance;
            if (amount > 1) amount = 1;
            fire.transform.position = Vector3.Lerp(startPos.position, endPos.position, amount);
            material.SetFloat(Fill, amount);
            yield return null;
        }

        onComplete?.Invoke();
        fire.gameObject.SetActive(false);
    }

    public void Recycle()
    {
        if (canFire) return;
        StartCoroutine(RecycleCor());
    }

    private IEnumerator RecycleCor()
    {
        while (amount > 0)
        {
            amount -= fireSpeed * Time.deltaTime / distance;
            if (amount < 0) amount = 0;
            fire.transform.position = Vector3.Lerp(startPos.position, endPos.position, amount);
            material.SetFloat(Fill, amount);
            yield return null;
        }
        onRecycle?.Invoke();
        canFire = true;
    }
}
