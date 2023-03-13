using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PaintStructure
{
    public Paintable paint;
    public Vector3 point;
    public float radius;
    public float hardness;
    public float strength;
    public Color? color;
}

public class PaintingObject : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private Color color;

    [SerializeField]
    private float eraseDistance = 1f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private int eraseQueueSize = 20;

    private float distanceChecker = 0f;
    private Vector3 prevPosition = Vector3.zero;
    Queue<PaintStructure> eraseQueue = new Queue<PaintStructure>();

    [SerializeField]
    private SphereCollider oilTrigger;
    private Queue<SphereCollider> colliders = new Queue<SphereCollider>();
    private List<SphereCollider> colliderList = new List<SphereCollider>();

    private void Start()
    {
        prevPosition = transform.position;

        Transform root = new GameObject("Oil Trigger Root").transform;

        for (int i = 0; i < eraseQueueSize; i++)
        {
            SphereCollider col = Instantiate(oilTrigger, root);
            colliders.Enqueue(col);
            colliderList.Add(col);
            col.gameObject.SetActive(false);
            col.radius = radius * 0.5f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask) != 0) return;

        Paintable p = collision.gameObject.GetComponent<Paintable>();

        if (!p) return;

        if (distanceChecker > eraseDistance)
        {
            if (eraseQueue.Count >= eraseQueueSize)
            {
                PaintStructure top = eraseQueue.Dequeue();

                if (!IsNear(top.point))
                {
                    StartCoroutine(DryCoroutine(top));
                }
            }

            SphereCollider col = colliders.Dequeue();
            col.transform.position = collision.GetContact(0).point;
            col.gameObject.SetActive(true);
            colliders.Enqueue(col);

            PaintManager.instance.Paint(p, collision.GetContact(0).point, radius, 0.2f, 1f, color);

            PaintStructure paint;
            paint.paint = p;
            paint.point = collision.GetContact(0).point;
            paint.radius = radius;
            paint.hardness = 0.2f;
            paint.strength = 1f;
            paint.color = color;

            eraseQueue.Enqueue(paint);

            distanceChecker = 0f;
        }


        distanceChecker += Vector3.Distance(prevPosition, transform.position);
        prevPosition = transform.position;
    }

    private IEnumerator DryCoroutine(PaintStructure top)
    {
        float timer = 0f;
        //PaintManager.instance.Paint(top.paint, top.point, top.radius, top.hardness, top.strength, Color.clear);

        while (timer < 0.5f)
        {
            Color blend = Color.Lerp(top.color.Value, Color.clear, timer / 1f);
            PaintManager.instance.Paint(top.paint, top.point, top.radius * 0.75f, top.hardness, top.strength, blend);

            timer += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private bool IsNear(Vector3 pos)
    {
        var cols =
        colliderList.FindAll(x => x.gameObject.activeSelf).
        OrderBy(x => Vector3.Distance(x.transform.position, pos));

        return (Vector3.Distance(pos, cols.ToList()[1].transform.position) < radius * 0.3f);
    }
}
