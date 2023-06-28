using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Ping : MonoBehaviour
{
    private Material pointMat;
    private Transform mainCam;
    private Transform modelTransform;
    private Color pointColor = Color.white;
    private ParticleSystem spawnParticle;

    float duration = 0.2f;
    bool playingAnim = false;
    Pet pet;
    Coroutine delayC;

    private void Awake()
    {
        modelTransform = transform.GetChild(0);
        pointMat = transform.GetChild(0).Find("point").GetComponent<MeshRenderer>().material;
        spawnParticle = transform.Find("SpawnParticle").GetComponent<ParticleSystem>();
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        modelTransform.transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }

    public void InitPing(Pet _pet)
    {
        if (delayC != null) StopCoroutine(delayC);
        switch (_pet.GetPetType)
        {
            case PetType.OilPet:
                pointColor = Color.yellow;
                break;
            case PetType.FirePet:
                pointColor = Color.red;
                break;
            case PetType.StickyPet:
                pointColor = Color.green;
                break;
        }
        pointMat.color = pointColor;
        spawnParticle.startColor = pointColor;

        pet = _pet;
    }

    public void PingEffect()
    {
        playingAnim = true;
        spawnParticle.gameObject.SetActive(true);

        modelTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        modelTransform.DOScale(modelTransform.localScale * 10, 0.3f).OnComplete(() =>
        {
            if (gameObject.activeSelf)
                StartCoroutine(PosAnim());
        });
    }

    private IEnumerator PosAnim()
    {
        while (playingAnim)
        {
            modelTransform.transform.DOLocalMoveY(0.5f, duration);
            yield return new WaitForSeconds(duration);
            modelTransform.transform.DOLocalMoveY(0.0f, duration);
            yield return new WaitForSeconds(duration);
        }
    }

    public void SetPoint(Vector3 setPos)
    {
        if (!pet.Agent.isOnNavMesh) return;
        if (delayC != null)
        {
            StopCoroutine(delayC);
            delayC = null;
        }
        transform.position = Vector3.Lerp(transform.position, setPos, Time.deltaTime*100);
    }

    public void OffPoint()
    {
        if (!gameObject.activeSelf) return;
       delayC = StartCoroutine(DelayDie());
    }

    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(0.4f);
        Off();
        pet.ReleasePing(this);
    }

    public void Off()
    {
        transform.DOKill();
        playingAnim = false;
        modelTransform.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        spawnParticle.gameObject.SetActive(false);
        pet.ReleasePing(this);
    }

}
