using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossGameController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onClear;

    [SerializeField]
    private UnityEvent onFail;

    [SerializeField]
    private UnityEvent onStart;

    private bool isFail = false;

    [SerializeField]
    private bool onAwake = false;

    [SerializeField]
    private Pet[] originalPets;

    private void Awake()
    {
        EventManager.StartListening(EventName.BossFail, Fail);
        EventManager.StartListening(EventName.BossSuccess, Clear);
    }

    private void Start()
    {
        if (onAwake)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        onStart?.Invoke();
    }

    public void Clear(EventParam param)
    {
        onClear?.Invoke();
    }

    public void Fail(EventParam param)
    {
        if (isFail)
            return;

        isFail = true;
        onFail?.Invoke();

        foreach (Pet pet in originalPets)
        {
            pet.GetPet(GameManager.Instance.PlayerController.transform);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.BossFail, Fail);
        EventManager.StopListening(EventName.BossSuccess, Clear);
    }
}
