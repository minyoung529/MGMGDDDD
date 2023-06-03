using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimatorParameters : MonoBehaviour
{
    [SerializeField]
    private List<AnimatorParameter> animatorParams;

    private Animator animator;

    [SerializeField]
    private bool onAwake = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (onAwake)
        {
            ChangeAllParams();
        }
    }

    public void ChangeAllParams()
    {
        animatorParams?.ForEach(x => x.ChangeParameter(animator));
    }
}

[System.Serializable]
public class AnimatorParameter
{
    [SerializeField]
    private ParameterType type;

    [SerializeField]
    private string parameterName;

    [Header("Parameter Values")]
    // Change Value
    [SerializeField]
    private int integerValue;

    [SerializeField]
    private float floatValue;

    [SerializeField]
    private bool booleanValue;

    public void ChangeParameter(Animator animator)
    {
        if (animator == null) return;

        switch (type)
        {
            case ParameterType.Integer:
                animator.SetInteger(parameterName, integerValue);
                break;
            case ParameterType.Float:
                animator.SetFloat(parameterName, floatValue);
                break;
            case ParameterType.Trigger:
                animator.SetTrigger(parameterName);
                break;
            case ParameterType.Boolean:
                animator.SetBool(parameterName, booleanValue);
                break;
        }
    }

    public void SetValue(object obj)
    {
        switch (type)
        {
            case ParameterType.Integer:
                integerValue = (int)obj;
                break;
            case ParameterType.Float:
                floatValue = (float)obj;
                break;
            case ParameterType.Boolean:
                booleanValue = (bool)obj;
                break;
        }
    }
}

public enum ParameterType
{
    Integer,
    Float,
    Trigger,
    Boolean,

    Count
}