using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPortal : MonoBehaviour
{
    public int Identification { get; set; } = 0;

    public void Initialize(int index, ref Color[] colors)
    {
        Identification = index;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = colors[index];
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pet pet = collision.transform.GetComponent<Pet>();

        if (pet)
        {
            OnContactPet(pet);
        }
    }

    protected virtual void OnContactPet(Pet pet) { }
}
