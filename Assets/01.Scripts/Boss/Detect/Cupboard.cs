using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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

    [SerializeField] private PlayableDirector openTimeline;
    [SerializeField] private PlayableDirector closeTimeline;

    [SerializeField] private CinemachineVirtualCamera innerCam;

    private Animator anim;

    private float delayTime = 0.5f;
    private bool playerIn = false;
    public bool PlayerIn => playerIn;
    private PlayerController player = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        innerCam.gameObject.SetActive(false);
    }

    public void InCupboard()
    {
        if (playerIn || player == null) return;
        openTimeline.Play();

        playerIn = true;
        anim.SetTrigger("Trigger");
        player.transform.position = inPos.position;

        EventParam param = new();
        param["State"] = false;
        EventManager.TriggerEvent(EventName.InPlayerCupboard, param);
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
        closeTimeline.Play();

        innerCam.gameObject.SetActive(false);
        player = null;
        playerIn = false;
        anim.SetTrigger("Close");

        EventParam param = new();
        param["State"] = true;
        EventManager.TriggerEvent(EventName.OutPlayerCupboard, param);
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