using DG.Tweening;
using UnityEditor.PackageManager;
using UnityEngine;

public class OilPaint : MonoBehaviour{

    [SerializeField] Color paintColor;
    [SerializeField] PhysicMaterial oil;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] ParticleSystem splashParticle;

    private Rigidbody rigid;

    private Vector3 defaultScale;

    private bool isBurn = false;
    public float scaleUp = 1.5f;

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
    }

    private void ResetBullet()
    {
        transform.localScale = defaultScale;

        isBurn = false;
        fireParticle.Stop();

        rigid.useGravity = false;
    }

    public void SetBurn()
    {
        isBurn = true;
        fireParticle.Play();
    }

    private void SpreadOil()
    {
        if(splashParticle.isPlaying) splashParticle.Stop();
        splashParticle.Play();

        rigid.velocity = Vector3.zero;
        transform.DOScale(defaultScale + new Vector3(scaleUp, scaleUp, scaleUp), 1f);
    }

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
       if(!other.CompareTag("OilPet")) SpreadOil();
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
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        other.material = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Paintable[] paints = collision.collider.GetComponents<Paintable>();

        Vector3 pos = collision.contacts[0].point;
        foreach (Paintable p in paints)
        {
            transform.DOKill();
            PaintManager.Instance.paint(p, pos, radius, hardness, strength, paintColor);
            SpreadOil();
        }
    }

    #endregion
}
