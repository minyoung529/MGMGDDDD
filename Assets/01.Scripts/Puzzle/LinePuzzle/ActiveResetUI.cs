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

        InputManager.StartListeningInput(InputAction.Down_Pet, ChangePet);
        InputManager.StartListeningInput(InputAction.Previous_Pet, ChangePet);
        InputManager.StartListeningInput(InputAction.Select_First_Pet, ChangePet);
        InputManager.StartListeningInput(InputAction.Select_Second_Pet, ChangePet);
        InputManager.StartListeningInput(InputAction.Select_Third_Pet, ChangePet);
        InputManager.StartListeningInput(InputAction.Up_Pet, ChangePet);

    }

    public void Inactive()
    {
        onInactive?.Invoke();
        gameObject.SetActive(false);

        InputManager.StopListeningInput(InputAction.Down_Pet, ChangePet);
        InputManager.StopListeningInput(InputAction.Previous_Pet, ChangePet);
        InputManager.StopListeningInput(InputAction.Select_First_Pet, ChangePet);
        InputManager.StopListeningInput(InputAction.Select_Second_Pet, ChangePet);
        InputManager.StopListeningInput(InputAction.Select_Third_Pet, ChangePet);
        InputManager.StopListeningInput(InputAction.Up_Pet, ChangePet);
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
    }
}
