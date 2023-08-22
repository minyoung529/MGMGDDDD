using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeDecal : MonoBehaviour
{
    [SerializeField] private DecalProjector decal;
    [SerializeField] private bool playOnAwake = true;


    private void Awake()
    {
        decal ??= GetComponent<DecalProjector>();

        if (playOnAwake) OnDecal();
        else OffDecal();
    }

    public void OnDecal()
    {
        StartCoroutine(Fade());
    }

    public void OffDecal()
    {
        decal.fadeFactor = 0;
    }

    private IEnumerator Fade()
    {
        while(decal.fadeFactor < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            decal.fadeFactor += 0.05f;
        }
        
    }
}
