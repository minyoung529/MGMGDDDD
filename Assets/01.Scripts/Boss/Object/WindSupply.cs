using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindSupply : MonoBehaviour
{
    [SerializeField] private float delayTime = 4f;
    [SerializeField] private StickyPet sticky;
    [SerializeField] private BreakableWindow cage;

    private Vector3 originScale = Vector3.zero;
    public void Trigger()
    {
        originScale = sticky.transform.localScale;
        StartCoroutine(AddScale());
    }

    private IEnumerator AddScale()
    {
        yield return new WaitForSeconds(delayTime);
        //sticky.State.ChangeState((int)PetStateName.Skill);
        sticky.Event.TriggerEvent((int)PetEventName.OnSkillKeyPress);
    }
    
    public void BreakCage()
    {
        sticky.transform.SetParent(null);
    }

}
