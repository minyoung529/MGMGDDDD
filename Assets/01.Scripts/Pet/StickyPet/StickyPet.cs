using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet
{
    private float moveSpeed = 1f;

    protected override void Awake()
    {
        base.Awake();

    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

    }

    #endregion

    #region Skill

    // Active Skill
    protected override void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);


    }

    #endregion

}
