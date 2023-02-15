using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] bool isDestroyType = false;

    bool isReadyBurn = false;
    bool isBurn = false;
    float burningTime = 2f;
    float burningReadyTime = 2f;

    public bool IsBurn { get { return isBurn; } }

    private void Awake()
    {
        isBurn = false;
    }

    public void Burn()
    {
        isBurn = true;
        if (isDestroyType) DestroyBurn();
    }

    public void StopBurn()  
    {
        isBurn = false;
    }

    public void DestroyBurn()
    {
        Destroy(gameObject, burningTime);
    }

    public void StayFire()
    {
        isReadyBurn = true;
        StartCoroutine(StayInFire());
    }
    private void ExitInFire()
    {
        if (isBurn) return;
        isReadyBurn = false;
        StopCoroutine(StayInFire());
    }


    private IEnumerator StayInFire()
    {
        yield return new WaitForSeconds(burningReadyTime);  

        if (isReadyBurn)
        {
            Burn();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        IceMelting[] ices = other.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        IceMelting[] ices = collision.collider.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }
    }

    //}  private void OnTriggerEnter(Collider other)
    //{
    //    Fire fire = other.GetComponent<Fire>();
    //    if(fire != null )
    //    {
    //        fire.StayFire();
    //    }

    //    IceMelting[] ices = other.GetComponents<IceMelting>();
    //    foreach (IceMelting ice in ices)
    //    {
    //        ice.Melt();
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    Fire fire = other.GetComponent<Fire>();
    //    if( fire != null )
    //    {
    //        fire.ExitInFire();
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Fire fire = collision.collider.GetComponent<Fire>();
    //    if (fire != null)
    //    {
    //        fire.StayFire();
    //    }

        
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    Fire fire = collision.collider.GetComponent<Fire>();
    //    if (fire != null)
    //    {
    //        fire.ExitInFire();
    //    }
    //}
}
