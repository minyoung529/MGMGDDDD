using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupboard : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private TutorialTrigger inTutorialTip;
    [SerializeField] private TutorialTrigger outTutorialTip;

    private bool playerIn = false;
    private float delayTime = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        SetEnableTutorial(false);
    }

    public void InCupboard()
    {
        if (playerIn) return;

        anim.SetTrigger("Trigger");
        SetEnableTutorial(true);
        GameManager.Instance.PlayerController.Move.LockInput();
    }

    public void OutCupboard()
    {
        if (!playerIn) return;

        anim.SetTrigger("Trigger");
        SetEnableTutorial(false);
        GameManager.Instance.PlayerController.Move.UnLockInput();
    }

    private void SetEnableTutorial(bool isIn)
    {
        Debug.Log(isIn);
        playerIn = isIn;
        if(isIn)
        {
            EventManager.TriggerEvent(EventName.InPlayerCupboard);
            
            inTutorialTip.EndTutorial();
            outTutorialTip.StartTutorial();
            inTutorialTip.Inactive();
            outTutorialTip.Active();
        }
        else
        {
            EventManager.TriggerEvent(EventName.OutPlayerCupboard);
            
            inTutorialTip.StartTutorial();
            outTutorialTip.EndTutorial();
            inTutorialTip.Active();
            outTutorialTip.Inactive();
            
        }
    }

}
