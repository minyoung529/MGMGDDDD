using DG.Tweening;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet {
    [SerializeField] private ParticleSystem skillEffect;

    private bool isSticky = false;
    private bool isHardMove = false;
    private float moveSpeed = 1f;

    private Sticky sticky = null;

    protected override void Awake() {
        base.Awake();

    }

    #region Set
    protected override void ResetPet() {
        base.ResetPet();

    }

    #endregion

    #region Skill

    // Active Skill
    protected override void Skill(InputAction inputAction, float value) {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);

        if (isSticky) {
            NotSticky();
        }
        else {
            SetSticky();
        }

    }

    private void SetSticky() {
        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero) {
            isSticky = true;

            StopClickMove();
            StopFollow();

            transform.DOMoveX(hit.x, moveSpeed);
            transform.DOMoveZ(hit.z, moveSpeed).OnComplete(() => {
                StartCoroutine(CheckDelay());
            });
        }
    }

    private IEnumerator CheckDelay() {
        yield return new WaitForSeconds(0.1f);
        if (sticky == null) {
            isSticky = false;
        }
    }

    private void NotSticky() {
        if (!isSticky) return;
        isSticky = false;

        IsNotMove = false;
        Destroy(GetComponent<FixedJoint>());
        sticky.NotSticky();

        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;

        isSticky = false;
        sticky = null;
    }

    private void StickyToCollision(Sticky stickyObject) {
        if (!isSticky) return;

        stickyObject.SetSticky();
        sticky = stickyObject;

        skillEffect.Play();
        Rigid.isKinematic = true;

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = sticky.GetComponent<Rigidbody>();
    }

    private void IsHard(HardMoveObject hard) {
        IsNotMove = !hard.CanMove;
    }

    private void OnCollisionEnter(Collision collision) {
        if (isSticky) {
            Sticky stickyObject = collision.collider.GetComponent<Sticky>();
            if (stickyObject != null) StickyToCollision(stickyObject);
            else isSticky = false;

            HardMoveObject hardObj = collision.collider.GetComponent<HardMoveObject>();
            if (hardObj != null) IsHard(hardObj);
        }
    }

    #endregion

}