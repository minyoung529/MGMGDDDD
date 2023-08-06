using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 
/// ���� : �÷��̾ ������ ���� ���� ����
/// 
/// Collider ������ ���� ���� ����
/// In : �����⸦ ����, �÷��̾� ���߱� + ������ ����
/// Out : ������ ������, �÷��̾� Ǯ���ֱ�
/// 
/// </summary>
public class Cupboard : MonoBehaviour
{
    [SerializeField] private Transform inPos;
    [SerializeField] private TutorialTrigger inTutorialTip;
    [SerializeField] private TutorialTrigger outTutorialTip;

    [SerializeField] private CinemachineVirtualCamera innerCam;

    private Animator anim;

    private float delayTime = 0.5f;
    private bool playerIn = false;
    private PlayerController player = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        innerCam.gameObject.SetActive(false);
    }

    public void InCupboard()
    {
        if (playerIn || player == null) return;

        playerIn = true;
        anim.SetTrigger("Trigger");
        player.transform.position = inPos.position;
        EventManager.TriggerEvent(EventName.InPlayerCupboard);
        GameManager.Instance.PlayerController.Move.LockInput();

        SetEnableTutorial(inTutorialTip, false);
        StartCoroutine(DelayOutTrigger());
        innerCam.gameObject.SetActive(true);
    }

    private IEnumerator DelayOutTrigger()
    {
        yield return new WaitForSeconds(delayTime);

        SetEnableTutorial(outTutorialTip, true);
    }

    public void OutCupboard()
    {
        if (!playerIn) return;

        innerCam.gameObject.SetActive(false);
        player = null;
        playerIn = false;
        anim.SetTrigger("Close");
        EventManager.TriggerEvent(EventName.OutPlayerCupboard);
        GameManager.Instance.PlayerController.Move.UnLockInput();

        SetEnableTutorial(outTutorialTip, false);
    }

    private void SetEnableTutorial(TutorialTrigger tuto, bool listen)
    {
        if (listen)
        {
            tuto.Active();
            tuto.StartTutorial();
        }
        else
        {
            tuto.Inactive();
            tuto.EndTutorial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerIn) return;
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            SetEnableTutorial(inTutorialTip, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetEnableTutorial(inTutorialTip, false);
        }
    }

}