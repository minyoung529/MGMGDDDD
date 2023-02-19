using DG.Tweening;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OilPaint : MonoBehaviour{

    [SerializeField] Color paintColor;
    [SerializeField] PhysicMaterial oil;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] ParticleSystem splashParticle;

    private Rigidbody rigid;
    private MeshRenderer mesh;

    private Vector3 defaultScale;

    private bool isBurn = false;
    private bool isSpread = false;
    public float scaleUp = 10f;

    public float radius = 0.5f;
    public float strength = 1;
    public float hardness = 1;

    private void OnEnable()
    {
        defaultScale = transform.localScale;
        ResetBullet();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mesh= GetComponent<MeshRenderer>();
    }

    private void ResetBullet()
    {
        transform.localScale = defaultScale;

        isBurn = false;
        isSpread= false;
        fireParticle.Stop();

        rigid.useGravity = false;
    }

    public void SetBurn()
    {
        isBurn = true;
        fireParticle.Play();
    }

    private void SpreadOil(Transform parent, Vector3 pos)
    {
        if (isSpread) return;
        isSpread = true;
        if (splashParticle.isPlaying) splashParticle.Stop();
        splashParticle.Play();

        rigid.isKinematic = true;
        rigid.detectCollisions = true;
        rigid.velocity = Vector3.zero;

        transform.DOScale(transform.localScale + new Vector3(scaleUp, scaleUp, scaleUp), 0.1f).OnComplete(()=>
        {
            HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        StartCoroutine(DestroyObj());
        });
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(60f);
        Destroy(gameObject.GetComponent<HingeJoint>());
        gameObject.SetActive(false);
    }

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG) || other.CompareTag(Define.PLAYER_TAG) || other.CompareTag(Define.OIL_PET_TAG)) return;
        SpreadOil(other.transform, transform.position);
    }
    private void OnTriggerStay(Collider other)
    {
        if(isBurn)
        {
            OilPaint[] oils = other.GetComponents<OilPaint>();

            foreach (OilPaint o in oils)
            {
                if (o.isBurn) continue;
                transform.DOKill();
                o.SetBurn();
            }

            TorchLight[] lights = other.GetComponents<TorchLight>();

            for(int i=0;i< lights.Length;i++)
            {
                if (lights[i].IsOn) continue;
                lights[i].OnLight();
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        other.material = null;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Paintable[] paints = collision.collider.GetComponents<Paintable>();

    //    Vector3 pos = collision.contacts[0].point;
    //    foreach (Paintable p in paints)
    //    {
    //        transform.DOKill();
    //     //   PaintManager.Instance.paint(p, pos, radius, hardness, strength, paintColor);
    //        SpreadOil();
    //    }
    //}

    #endregion
}
