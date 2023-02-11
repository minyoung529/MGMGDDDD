using UnityEngine;

public class OilPaint : MonoBehaviour{

    [SerializeField] Color paintColor;
    [SerializeField] PhysicMaterial oil;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] ParticleSystem splashParticle;

    private Rigidbody rigid;
    private SphereCollider col;
    private MeshRenderer meshRender;

    private bool isBurn = false;
    public float radius = 0.5f;
    public float strength = 1;
    public float hardness = 1;

    private void OnEnable()
    {
        ResetBullet();
    }

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        meshRender = GetComponent<MeshRenderer>();
    }

    private void ResetBullet()
    {
        isBurn = false;
        col.radius = 0.5f;
        fireParticle.Stop();
        col.isTrigger = false;
        meshRender.enabled = true;
    }

    public void SetBurn()
    {
        isBurn = true;
        fireParticle.Play();
    }

    private void SpreadOil()
    {
        if(splashParticle.isPlaying) fireParticle.Stop();

        col.isTrigger = true;
        col.radius = 1.0f;
        meshRender.enabled = false;
    }

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
        other.material = oil;
    }
    private void OnTriggerExit(Collider other)
    {
        other.material = null;
    }

    private void OnCollisionStay(Collision collision)
    {
        Paintable[] paints = collision.collider.GetComponents<Paintable>();

        Vector3 pos = collision.contacts[0].point;
        foreach (Paintable p in paints)
        {
            PaintManager.Instance.paint(p, pos, radius, hardness, strength, paintColor);
            SpreadOil();
        }
    }

    #endregion
}
