using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetUI : MonoBehaviour
{
    [SerializeField] private List<Image> petInvens = new List<Image>();

    public Vector3 SelectScale => new Vector3(1.25f, 1.25f, 1.25f);
    public Vector3 OriginScale => new Vector3(1f, 1f, 1f);

    private Dictionary<PetType, Image> invens = new Dictionary<PetType, Image>();

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        ResetPetUI();
    }

    public void ResetPetUI()
    {
        foreach(var inven in invens)
        {
            if(invens.ContainsKey(PetType.OilPet)) invens[PetType.OilPet].gameObject.SetActive(false);
            if(invens.ContainsKey(PetType.FirePet)) invens[PetType.FirePet].gameObject.SetActive(false);
            if (invens.ContainsKey(PetType.StickyPet)) invens[PetType.StickyPet].gameObject.SetActive(false);


        }
        invens.Clear();

        invens.Add(PetType.FirePet, petInvens[0]);
        invens.Add(PetType.OilPet, petInvens[1]);
        invens.Add(PetType.StickyPet, petInvens[2]);

        invens[PetType.OilPet].gameObject.SetActive(false);
        invens[PetType.FirePet].gameObject.SetActive(false);
        invens[PetType.StickyPet].gameObject.SetActive(false);
    }

    public void SelectPetUI(PetType type)
    {
        for (int i = 0; i < PetManager.Instance.PetCount; i++)
        {
            OffInven(PetManager.Instance.GetPetList[i].GetPetType);
        }
        OnInven(type);
    }

    private void OnInven(PetType type)
    {
        if (!invens.ContainsKey(type)) return;
        invens[type].DOFade(1f, 0.5f);
        invens[type].transform.DOScale(SelectScale, 0.5f).SetEase(Ease.Flash);
    }
    private void OffInven(PetType type)
    {
        if (!invens.ContainsKey(type)) return;
        invens[type].DOFade(0.2f, 0.5f);
        invens[type].transform.DOScale(OriginScale, 0.5f);
    }

    public void Active(PetType type)
    {
        if (!invens.ContainsKey(type)) return;
        invens[type].gameObject.SetActive(true);
    }

    public void Inactive(PetType type)
    {
        if (!invens.ContainsKey(type)) return;
        invens[type].gameObject.SetActive(false);
    }

    public void ActivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void InactivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}