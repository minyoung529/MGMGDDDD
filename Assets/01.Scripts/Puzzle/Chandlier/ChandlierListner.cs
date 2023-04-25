using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandlierListner : MonoBehaviour
{
    private Action<ChandlierListner> onLighting;
    private DetectOil oilDetector;

    private Fire fire;


    private bool isTouched = false;
    private bool touchCalculating = false;
    private float touchedTime;

    [SerializeField]
    private ParticleSystem[] particles;

    #region Property
    public bool IsOilContact => oilDetector.IsContactOil;
    public bool IsTouched => isTouched;
    public float TouchedTime
    {
        set
        {
            touchedTime = value;
        }
    }

    public bool IsSuccess = false;

    #endregion

    private void Awake()
    {
        oilDetector = GetComponent<DetectOil>();
        fire = GetComponent<Fire>();

        fire.ListeningFireEvent(Lighting);
    }

    public void ListeningOnLighting(Action<ChandlierListner> action)
    {
        onLighting += action;
    }

    public void Lighting()
    {
        onLighting?.Invoke(this);
    }

    public void Fire()
    {
        fire.Burn();
        foreach (var particle in particles)
        {
            particle.Play();
        }

        IsSuccess = true;
    }

    public void StopFire()
    {
        fire.StopBurn();
        IsSuccess = false;
    }

    public void BlockLighting()
    {
        if (!touchCalculating)
        {
            StartCoroutine(TurnOffIsTouched());
        }
    }

    private IEnumerator TurnOffIsTouched()
    {
        isTouched = true;
        touchCalculating = true;
        yield return new WaitForSeconds(touchedTime);
        isTouched = false;
        touchCalculating = false;
    }
}
