using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineKey : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] madeKeyParticle;
    [SerializeField] private GameObject[] keyPieceList;
    [SerializeField] private GameObject key;

    private void Awake()
    {
        key.SetActive(false);
    }

    public void Made()
    {
        for (int i = 0; i < madeKeyParticle.Length; i++)
        {
            madeKeyParticle[i].Play();
        }

        for (int i = 0; i < keyPieceList.Length; i++)
        {
            keyPieceList[i].SetActive(false);
        }

        key.SetActive(true);
    }
}
