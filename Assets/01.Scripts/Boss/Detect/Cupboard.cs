using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UIElements;
/// <summary>
/// 
/// 벽장 : 플레이어가 보스를 피해 숨을 공간
/// 
/// Collider 가까이 가면 들어가기 띄우기
/// In : 나가기를 띄우기, 플레이어 멈추기 + 움직임 고정
/// Out : 나가기 내리기, 플레이어 풀어주기
/// 
/// </summary>
public class Cupboard : MonoBehaviour
{
    [SerializeField] private Transform inPos;
    [SerializeField] private Transform outPos;
    [SerializeField] private TutorialTrigger inTutorialTip;

    [SerializeField] private CinemachineVirtualCamera innerCam;

    private Animator anim;

    private bool playerIn = false;
    public bool PlayerIn => playerIn;
    private PlayerController player = null;

    [SerializeField]
    private bool playerDetect = false;

    [SerializeField]
    private UnityEvent onInCupBoard;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        innerCam.Priority = 0;
    }

    public void InCupboard()
    {
        if (playerIn || player == null)
        {
            SetEnableTutorial(inTutorialTip, true);
            return;
        }

        inTutorialTip.EndTutorial();
        SetEnableTutorial(inTutorialTip, false);
        anim.SetBool("Open", true);
        innerCam.Priority = 1000;
        player.transform.position = inPos.position;
        playerIn = true;

        EventParam param = new();
        param["State"] = false;
        EventManager.TriggerEvent(EventName.InPlayerCupboard, param);
        GameManager.Instance.PlayerController.Move.LockInput();

        List<Pet> petList = PetManager.Instance.GetPetList;
        petList.ForEach(x => x.gameObject.SetActive(false));

        onInCupBoard?.Invoke();
    }

    public void OutCupboard()
    {
        if (!playerIn)
        {
            return;
        }

        player.transform.position = outPos.position;
        player = null;
        playerIn = false;
        innerCam.Priority = 0;
        anim.SetBool("Open", false);

        EventParam param = new();
        param["State"] = true;
        EventManager.TriggerEvent(EventName.OutPlayerCupboard, param);
        GameManager.Instance.PlayerController.Move.UnLockInput();

        List<Pet> petList = PetManager.Instance.GetPetList;
        petList.ForEach(x => x.gameObject.SetActive(true));
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
        if (playerDetect) return;

        if (other.CompareTag("Player"))
        {
            playerDetect = true;
            player = other.GetComponent<PlayerController>();
            SetEnableTutorial(inTutorialTip, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerDetect) return;

        if (other.CompareTag("Player"))
        {
            playerDetect = false;
            SetEnableTutorial(inTutorialTip, false);
        }
    }
}