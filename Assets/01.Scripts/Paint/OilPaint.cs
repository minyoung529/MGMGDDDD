using UnityEngine;

public class OilPaint : MonoBehaviour{
    public Color paintColor;
    public PhysicMaterial oil;
    
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

    private void OnCollisionEnter(Collision other) {
        if (other.collider.TryGetComponent(out Paintable p)){
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            SpreadOil();
        }

    }

    void SpreadOil()
    {
      //  rigid.useGravity= false;
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
