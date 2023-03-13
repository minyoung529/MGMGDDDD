using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet
{
    [SerializeField] ParticleSystem skillEffect;

    private bool isSticky = false;
    private float moveSpeed = 1f;

    private Sticky sticky = null;
    private FixedJoint joint = null;

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

        if (isSticky)
        {
            NotSticky();
        }
        else
        {
            SetSticky();
        }
      
    }

    private void SetSticky()
    {
        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            isSticky = true;

            StopClickMove();
            StopFollow();

            transform.DOMoveX(hit.x, 1.0f);
            transform.DOMoveZ(hit.z, 1.0f);
        }
    }

    private void NotSticky()
    {
        Destroy(joint);

        sticky.NotSticky();
        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;

        isSticky = false;

        joint = null;
        sticky = null;
    }
    
    private void StickyToCollision(Sticky stickyObject)
    {
        stickyObject.SetSticky();
        sticky = stickyObject;

        skillEffect.Play();
        Rigid.isKinematic = true;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = sticky.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSticky) return;

        Sticky stickyObject = collision.collider.GetComponent<Sticky>();
        if(stickyObject != null) StickyToCollision(stickyObject);

    }
    #endregion

}
