using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Ping : MonoBehaviour
{
    private Material pointMat;
    private Transform mainCam;
    private Animator anim;
    private Color pointColor = Color.white;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        pointMat = transform.GetChild(0).Find("point").GetComponent<MeshRenderer>().material;
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }

    public void InitPing(PetType petType)
    {
        switch (petType)
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
        gameObject.SetActive(false);
    }

    public void SetPoint(Vector3 setPos)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        anim.SetBool("Positioning", true);
        transform.position = setPos;
    }

    public void OffPoint()
    {
        StartCoroutine(Off());
    }

    private IEnumerator Off()
    {
        anim.SetTrigger("End");
        anim.SetBool("Positioning", false);
        yield return new WaitForSeconds(2f);
        anim.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.SetActive(false);
    }

}
