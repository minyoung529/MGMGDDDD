using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BushPuzzle : MonoBehaviour
{
    public UnityEvent bushEvent;
    [SerializeField] private GameObject[] bushes;
    [SerializeField] private ParticleSystem[] fires;

    [SerializeField] new private Light light;
    new private Collider collider;

    Fire fire;

    private void Start()
    {
        collider = GetComponent<Collider>();
        fire= GetComponent<Fire>();
    }


    public void OffBurn()
    {
        fire.StopBurn();

        light.DOIntensity(0f, 0.5f).OnComplete(() => light.gameObject.SetActive(false));
    }

}
