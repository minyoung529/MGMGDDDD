using UnityEngine;

public class OilPaint : MonoBehaviour{
    [SerializeField] ParticleSystem splashParticle;
    [SerializeField] Color paintColor;
    [SerializeField] PhysicMaterial oil;
    
    public float radius = 0.5f;
    public float strength = 1;
    public float hardness = 1;

    Rigidbody rigid;
    SphereCollider col;
    MeshRenderer render;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        render = GetComponent<MeshRenderer>();
    }

    private void OnCollisionStay(Collision collision)
    {
        Paintable p = collision.collider.GetComponent<Paintable>();
        if (p != null)
        {
            Debug.Log("Paint");
            Vector3 pos = collision.contacts[0].point;
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            SpreadOil();
        }
    }

    void SpreadOil()
    {
        splashParticle.Play();
        rigid.useGravity= false;
        col.isTrigger = true;

        col.radius = 1.0f;
        render.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.material = oil;
    }
    private void OnTriggerExit(Collider other)
    {
        other.material = null;
    }
}
