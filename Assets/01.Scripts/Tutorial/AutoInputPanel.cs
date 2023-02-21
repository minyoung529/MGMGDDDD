using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AutoInputPanel : MonoBehaviour
{
    [Header("Animator Trigger")]
    [SerializeField]
    private string animatorBoolName;

    [SerializeField]
    private bool isBool = true;

    [SerializeField]
    private int intValue = 1;

    [Header("Input")]
    [SerializeField] private bool useInputAction = false;
    [SerializeField] InputAction action;

    private Animator animator;

    private bool active;
    public bool Active { get { return active; } set { active = value; } }

    private bool success = false;
    public bool Success { get { return success; } }

    #region TIME
    [Header("Check")]
    [SerializeField]
    private bool checkTime = false;

    private float timer = 0f;
    private readonly float MAX_TIME = 2f;

    [SerializeField]
    private List<Image> fillImages;

    [SerializeField]
    private bool isFill = false;
    #endregion

    private readonly Color MAX_COLOR = new Color32(72, 204, 70, 69);

    private void Start()
    {
        if (useInputAction)
            InputManager.StartListeningInput(action, SuccessInput);
    }

    public void Init(Animator animator)
    {
        this.animator = animator;
    }

    void Update()
    {
        if ((string.IsNullOrEmpty(animatorBoolName) || animatorBoolName == ""))
        {
            if (!useInputAction)
            {
                success = true;
                return;
            }
            else return;
        }

        if (Active && (isBool && animator.GetBool(animatorBoolName) || (!isBool && animator.GetInteger(animatorBoolName) == intValue)))
        {
            if (checkTime)
            {
                timer += Time.deltaTime;

                if (isFill)
                {
                    SetFadeColor();
                }

                if (timer > MAX_TIME)
                {
                    // SUCCESS
                    success = true;
                }
            }
            else
            {
                // SUCCESS
                success = true;
            }
        }
    }

    private void SetFadeColor()
    {
        foreach (Image image in fillImages)
        {
            image.color = Color.Lerp(Color.clear, MAX_COLOR, timer / MAX_TIME);
        }
    }

    private void SuccessInput(InputAction inputAction, float value)
    {
        success = true;
    }

    private void OnDestroy()
    {
        if (useInputAction)
            InputManager.StopListeningInput(action, SuccessInput);
    }
}