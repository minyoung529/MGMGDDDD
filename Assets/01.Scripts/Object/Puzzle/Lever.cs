using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool isRotate = true;
    public UnityEvent OnLever;
    public UnityEvent OffLever;
    public bool disposable = true;
    public LayerMask playerLayer;

    private Transform handle;

    private bool isNear = false;
    private bool toggle = false;

    private void Start()
    {
        StartListen();
        SetLever();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Interaction, ToggleEvent);
    }

    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Interaction, ToggleEvent);
    }

    protected virtual bool CheckLever()
    {
        return NearPlayer();
    }

    protected virtual void SetLever()
    {
       //OnLever.AddListener(DebugOnLever);
       //OffLever.AddListener(DebugOffLever);

        handle = transform.GetChild(0);
    }

    #region Boolean
    // ?????? ?????? ??????? u???? ???
    protected bool NearPlayer()
    {
        if (isNear) return true;
        return false;
    }
    #endregion

    #region Event
    // Event Toggle
    [ContextMenu("Lever")]

    private void ToggleEvent(InputAction action, float value)
    {
        if (!CheckLever()) return;

        toggle = !toggle;
        if(disposable) toggle = true;

        if (toggle)
        {
            EventStart();
        }
        else
        {
            EventStop();
        }
    }

    protected virtual void EventStart()
    {
        OnRotateLever();
        OnLever.Invoke();
    }
    protected virtual void EventStop()
    {
        OffRotateLever();
        OffLever.Invoke();
    }
    #endregion

    #region RotateLever
    private void OnRotateLever()
    {
        if (!isRotate) return;

        handle.DOKill();
        handle.DOLocalRotate(new Vector3(0f, 0f, -45f), 1f);
    }
    private void OffRotateLever()
    {
        if (!isRotate) return;

        handle.DOKill();
        handle.DOLocalRotate(new Vector3(0f, 0f, 45f), 1f);
    }
    #endregion

    #region Debug
    public void DebugOnLever()
    {
        Debug.Log("On Lever");
    }
    public void DebugOffLever()
    {
        Debug.Log("Off Lever");
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = false;
        }
    }

    private void OnDestroy()
    {
        StopListen();
    }
}
