using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class ActiveResetUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas;

    [SerializeField]
    private UnityEvent onActive;

    [SerializeField]
    private UnityEvent onInactive;

    [SerializeField]
    private UnityEvent onChangePet;

    [SerializeField]
    private Sprite firePet;
    [SerializeField]
    private Sprite oilPet;

    [SerializeField]
    private Image petIcon;

    public void Active()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(2f);
        seq.AppendCallback(Show);

        PetManager.Instance.StartListen(InputAction.Down_Pet, ChangePet);
        PetManager.Instance.StartListen(InputAction.Select_First_Pet, ChangePet);
        PetManager.Instance.StartListen(InputAction.Select_Second_Pet, ChangePet);
        PetManager.Instance.StartListen(InputAction.Select_Third_Pet, ChangePet);
        PetManager.Instance.StartListen(InputAction.Up_Pet, ChangePet);
    }

    public void Inactive()
    {
        onInactive?.Invoke();
        gameObject.SetActive(false);

        PetManager.Instance.StopListen(InputAction.Down_Pet, ChangePet);
        PetManager.Instance.StopListen(InputAction.Select_First_Pet, ChangePet);
        PetManager.Instance.StopListen(InputAction.Select_Second_Pet, ChangePet);
        PetManager.Instance.StopListen(InputAction.Select_Third_Pet, ChangePet);
        PetManager.Instance.StopListen(InputAction.Up_Pet, ChangePet);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        canvas.DOFade(1f, 1f);
        ChangePet();

        onActive?.Invoke();
    }


    private void ChangePet(InputAction action = InputAction.Select_First_Pet, float value = 0f)
    {
        StartCoroutine(DelayChange());
    }

    private IEnumerator DelayChange()
    {
        yield return null;

        PetType type = PetManager.Instance.GetSelectPet.GetPetType;

        switch (type)
        {
            case PetType.FirePet:
                petIcon.sprite = firePet;
                break;

            case PetType.OilPet:
                petIcon.sprite = oilPet;
                break;

            default:
                petIcon.sprite = null;
                break;
        }

        onChangePet?.Invoke();
    }
}
