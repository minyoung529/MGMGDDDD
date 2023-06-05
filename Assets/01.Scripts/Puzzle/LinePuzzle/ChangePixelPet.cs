using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePixelPet : MonoBehaviour
{
    private Pet pet;

    private MeshRenderer[] meshRenderers;

    [SerializeField]
    private GameObject pixelPetPrefab;
    private GameObject pixelPet;

    [SerializeField]
    private PetType petType;

    private Quaternion rotation;

    void Start()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        if(pixelPet)
        {
            pixelPet.transform.rotation = rotation;
        }
    }

    public void AttachPixelPet()
    {
        switch (petType)
        {
            case PetType.OilPet:
                pet = PetManager.Instance.GetPetByKind<OilPet>();
                break;

            case PetType.FirePet:
                pet = PetManager.Instance.GetPetByKind<FirePet>();
                break;

            case PetType.StickyPet:
                pet = PetManager.Instance.GetPetByKind<StickyPet>();
                break;
        }

        if (pet == null) return;

        meshRenderers = pet.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        pixelPet = Instantiate(pixelPetPrefab);
        pixelPet.transform.SetParent(pet.transform);
        pixelPet.transform.localPosition = Vector3.zero;
    }

    public void DettachPixelPet()
    {
        if (pet == null) return;

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = true;
        }

        Destroy(pixelPet);
        pet = null;
    }
}
