using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PaintedOil : MonoBehaviour
{
    public event EventHandler OnContactFirePet;
    new private SphereCollider collider;
    public SphereCollider Collider { get { return collider; } }

    private Fire fire;

    [SerializeField]
    private GameObject fireEffect;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FirePet"))
        {
            OnContactFirePet?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Burn()
    {
        fire = gameObject.AddComponent<Fire>();
        fireEffect.SetActive(true);
    }

    public void ResetOil()
    {
        Destroy(fire);
        fire = null;

        gameObject.SetActive(false);
        fireEffect.SetActive(false);
    }

}