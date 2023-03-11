using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    [SerializeField] private PetTypeSO petInform;

    #region Bool

    private bool isGet = false;
    private bool isFollow = false;
    private bool isCoolTime = false;
    private bool isSelected = false;
    private bool isClickMove = false;

    #endregion

    #region Get

    public bool IsGet { get { return isGet; } }
    public bool IsCoolTime { get { return isCoolTime; } }
    public bool IsSelected { get { return isSelected; } }
    public float Distance { get { return Vector3.Distance(transform.position, target.position); } }
    public bool IsFollowDistance { get { return Vector3.Distance(transform.position, target.position) >= petInform.followDistance; } }
    public bool CheckSkillActive { get { return (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsCoolTime); } }
    public Sprite petSprite {  get { return petInform.petUISprite; } }
    public Animator Anim => anim;

    #endregion

    #region Component

    private Camera camera;
    private Animator anim;
    private Rigidbody rigid;
    private Transform target;
    private NavMeshAgent agent;

    #endregion

    private List<Action> actingFunc;

    private Vector3 destination = Vector3.zero;

    protected virtual void Awake()
    {
        camera = Camera.main;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!IsGet) return;

        Follow();
        LookAtPlayer();
    }

    protected virtual void ResetPet()
    {
        isGet = false;
        isFollow = false;
        isCoolTime = false;

        rigid.velocity = Vector3.zero;
        agent.velocity = Vector3.zero;

        InitActing();
        EmotionHide();
    }

    private void InitActing()
    {
        SetIdleActing(IdleActing);
    }

    protected void LookAtPlayer()
    {
        Vector3 dir = target.position;
        if (ThirdPersonCameraControll.IsPetAim)
        {
            dir = GameManager.Instance.GetCameraHit();
        }

        Quaternion targetRot = Quaternion.LookRotation((dir - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    public void Select(bool select)
    {
        isSelected = select;
    }

    #region Own

    public void GetPet(Transform obj)
    {
        isGet = true;
        target = obj;

        StartFollow();
        StartListen();
        PetManager.Instance.AddPet(this);
    }
    public void LosePet()
    {
        ResetPet();
        StopListen();
        PetManager.Instance.DeletePet(this);
    }


    #endregion
   
    #region Move

    protected void Follow()
    {
        if (isClickMove)
        {
            ClickMove();
        }
        else if (isFollow)
        {
            agent.SetDestination(target.position);
        }
    }

    #region ClickMove

    private void MovePoint(InputAction inputAction, float value)
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected) return;

        StopFollow();
        ClickSetDestination(GameManager.Instance.GetCameraHit());
    }
    private void ClickSetDestination(Vector3 dest)
    {
        isClickMove = true;
        destination = dest;
        rigid.velocity = Vector3.zero;
    }
    private void StopClickMove()
    {
        isClickMove = false;
        destination = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }
    private void ClickMove()
    {
        if (Vector3.Distance(destination, transform.position) <= 1f)
        {
            isClickMove = false;
            return;
        }
        var dir = destination - transform.position;
        dir.y = 0;
        transform.position += dir.normalized * Time.deltaTime * 5f;
    }

    #endregion

    #region AgentMove
    private void StartFollow(InputAction inputAction, float value)
    {
        isFollow = true;
    }
    private void StartFollow()
    {
        isFollow = true;
    }
    private void StopFollow()
    {
        isFollow = false;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }
    
    #endregion

    #endregion

    #region Skill
    protected virtual void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;

        SkillDelay();
    }

    protected void SkillDelay()
    {
        isCoolTime = true;
        StartCoroutine(SkillCoolTime(petInform.skillDelayTime));
    }
    private IEnumerator SkillCoolTime(float t)
    {
        yield return new WaitForSeconds(t);
        isCoolTime = false;
    }

    #endregion

    #region Throw

    private void InputThrow(InputAction input, float value)
    {
        Throw();
    }

    public void Hold()
    {
        Debug.Log(petInform.petType + " : Hold");
    }
    public void Throw()
    {
        Debug.Log(petInform.petType + " : Throw");
    }

    #endregion

    #region Acting

    private void SetIdleActing(Action act)
    {
        actingFunc.Add(act);
    }

    public virtual void RandomIdleActing()
    {
        int randomAct = UnityEngine.Random.Range(0, actingFunc.Count);
        actingFunc[randomAct].Invoke();
    }

    protected virtual void IdleActing()
    {

    }

    #endregion
    
    #region Emoji

    protected void Emotion()
    {

    }

    protected void EmotionShow()
    {

    }
    protected void EmotionHide()
    {

    }

    #endregion

    #region Animation

    public void AnimPlay(string animation)
    {
        anim.SetTrigger(animation);
    }
    public void AnimPlay(string animation, bool isPlay)
    {
        anim.SetBool(animation, isPlay);
    }

    #endregion

    #region InputSystem
    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StartListeningInput(InputAction.Pet_Throw, InputThrow);
        InputManager.StartListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StartListeningInput(InputAction.Pet_Follow, StartFollow);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StopListeningInput(InputAction.Pet_Throw, InputThrow);
        InputManager.StopListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StopListeningInput(InputAction.Pet_Follow, StartFollow);
    }
    #endregion
}
