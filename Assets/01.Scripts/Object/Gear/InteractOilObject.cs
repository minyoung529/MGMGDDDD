using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractOilObject : MonoBehaviour
{
    [SerializeField] private Material rustMaterial;
    new private Renderer renderer;

    private bool isRust = true;
    public bool IsRust => isRust;

    public UnityEvent OnEnterOil;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Rust(GameObject gameObject)
    {
        if (gameObject.CompareTag(Define.OIL_BULLET_TAG))
        {
            isRust = false;
            OnEnterOil.Invoke(); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRust) return;
        Rust(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isRust) return;
        Rust(other.gameObject);
    }
}
