//using DG.Tweening;
//using UnityEditor.SceneManagement;
//using UnityEditor.ShaderKeywordFilter;
//using UnityEngine;

//public class StickyPet : Pet
//{
//    private float moveSpeed = 1f;

//    #region Set
//    protected override void ResetPet()
//    {
//        base.ResetPet();

//        rigid.useGravity = false;
//        rigid.isKinematic = false;
//        agent.enabled= false;
//    }

//    public override void AppearPet()
//    {
//        base.AppearPet();

//        rigid.useGravity = true;
//    }

//    #endregion

//    #region Skill

//    // Active Skill
//    protected override void ActiveSkill(InputAction inputAction, float value)
//    {
//        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsCoolTime) return;
//        base.ActiveSkill(inputAction, value);

//        RaycastHit hit;
//        if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
//        {
//            MoveActiveSkill(hit);
//        }
//    }
//    private void MoveActiveSkill(RaycastHit hit)
//    {
//        agent.enabled = false;

//        transform.DOKill();
//        transform.DOScale(new Vector3(1.5f , 1.5f, 1.5f), 1f);
//        transform.DOMove(hit.point, moveSpeed).OnComplete(()=>
//        {
//            StickySkill(hit.collider.gameObject);
//            SkillDelay();
//        });
//    }

//    private void StickySkill(GameObject obj)
//    {
//        Rigidbody rb = obj.GetComponent<Rigidbody>();
//        if (rb == null) rb = obj.AddComponent<Rigidbody>();

//        rb.isKinematic = false;
//        rb.detectCollisions = true;
//        rb.useGravity = false;
//        rigid.isKinematic = true;
//        rigid.detectCollisions = true;

//        FixedJoint joint = transform.gameObject.AddComponent<FixedJoint>();
//        joint.connectedBody = rb;
//    }


//    #endregion



//}
