using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private PlayerMove playerMove;
    private Pet holdingPet;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
        InputManager.StartListeningInput(InputAction.Interaction, GetInput);
    }

    private void GetInput(InputAction action, float value) {
        if (!holdingPet)
            PickUp();
        else
            Throw();
    }

    private void PickUp() {
        holdingPet = GameManager.Instance.GetNearest(transform, GameManager.Instance.Pets);
        Sequence seq = DOTween.Sequence();
        seq.Append(holdingPet.)
        playerMove.ChangeState(StateName.PickUp);
        seq.Kill();
    }

    public void PickUpEvent() {
        
    }

    private void Throw() {
        playerMove.ChangeState(StateName.Throw);
    }

    public void ThrowEvent() {

    }
}
