using DG.Tweening;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OilPaint : MonoBehaviour{

    [SerializeField] ParticleSystem splashParticle;
    private Rigidbody rigid;

    private Vector3 defaultScale;

    private bool isBurn = false;
    private bool isSpread = false;
    public float scaleUp = 10f;

    public bool IsBurn { get { return isBurn; } }
    private void OnEnable()
    {
        defaultScale = transform.localScale;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void ResetBullet()
    {
        transform.localScale = defaultScale;

        isBurn = false;
        isSpread= false;

        rigid.useGravity = false;
    }

    private void SpreadOil(Transform parent, Vector3 pos)
    {
        if (isSpread) return;
        isSpread = true;
        if (splashParticle.isPlaying) splashParticle.Stop();
        splashParticle.Play();

        transform.DOScale(transform.localScale + new Vector3(scaleUp, scaleUp, scaleUp), 0.1f).OnComplete(() =>
        {
            //HingeJoint joint = gameObject.AddComponent<HingeJoint>();
            transform.SetParent(parent);
            StartCoroutine(DelayDestroy());
        });

        rigid.isKinematic = true;
        rigid.detectCollisions = false;

    }
    public void BurnDestroy()
    {
        StartCoroutine(DestroyOil());
    }
    public IEnumerator DestroyOil()
    {
        yield return new WaitForSeconds(1f);
    //    Destroy(gameObject.GetComponent<HingeJoint>());
    }
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(180f);
        StartCoroutine(DestroyOil());
    }

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG) || other.CompareTag(Define.PLAYER_TAG) || other.CompareTag(Define.OIL_PET_TAG)) return;

        SpreadOil(other.transform, transform.position);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.FIRE_PET_TAG) || collision.collider.CompareTag(Define.PLAYER_TAG) || collision.collider.CompareTag(Define.OIL_PET_TAG)) return;

        SpreadOil(collision.transform, transform.position);
    }
   
    private void OnTriggerExit(Collider other)
    {
        other.material = null;
    }

    #endregion
}
