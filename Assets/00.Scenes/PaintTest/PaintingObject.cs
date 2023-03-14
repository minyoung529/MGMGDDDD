using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

struct PaintStructure
{
    public Paintable paint;
    public Vector3 point;
    public float radius;
    public float hardness;
    public float strength;
    public Color? color;

    public void DataSet(Paintable paint, Vector3 point, float radius, float hardness, float strength, Color? color)
    {
        this.paint = paint;
        this.point = point;
        this.radius = radius;
        this.hardness = hardness;
        this.strength = strength;
        this.color = color;
    }
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

    private readonly float OIL_PAINT_DURATION = 0.5f;

    private bool isPainting = false;
    public bool IsPainting
    {
        get
        {
            return isPainting;
        }
        set
        {
            prevPosition = transform.position;
            isPainting = value;
        }
    }

    private void Start()
    {
        prevPosition = transform.position;

        Transform root = new GameObject("-- Oil Trigger Root --").transform;

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
        if (!isPainting) return;
        if (((1 << collision.gameObject.layer) & layerMask) == 0) return;

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

            if (IsNear(collision.GetContact(0).point)) return;

            CreateOilPaint(collision, p);

            PaintStructure paint = new();
            paint.DataSet(p, collision.GetContact(0).point, radius, 0.2f, 1f, color);

            eraseQueue.Enqueue(paint);
            distanceChecker = 0f;
        }

        distanceChecker += Vector3.Distance(prevPosition, transform.position);
        prevPosition = transform.position;
    }

    void CreateOilPaint(Collision collision, Paintable p)
    {
        SphereCollider col = colliders.Dequeue();
        col.transform.position = collision.GetContact(0).point;
        col.gameObject.SetActive(true);
        colliders.Enqueue(col);

        PaintStructure paintData = new PaintStructure();
        paintData.DataSet(p, collision.GetContact(0).point, radius, 0.2f, 1f, color);

        StartCoroutine(SpreadCoroutine(paintData));
        //PaintManager.instance.Paint(p, collision.GetContact(0).point, radius, 0.2f, 1f, color);
    }

    private IEnumerator DryCoroutine(PaintStructure p)
    {
        float timer = 0f;

        while (timer < OIL_PAINT_DURATION)
        {
            Color blend = Color.Lerp(p.color.Value, Color.clear, timer / OIL_PAINT_DURATION);
            PaintManager.instance.Paint(p.paint, p.point, p.radius * 0.75f, p.hardness, p.strength, blend);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SpreadCoroutine(PaintStructure top)
    {
        float timer = 0f;

        while (timer < OIL_PAINT_DURATION)
        {
            Color blend = Color.Lerp(Color.clear, top.color.Value, timer / OIL_PAINT_DURATION);
            PaintManager.instance.Paint(top.paint, top.point, top.radius * 0.75f, top.hardness, top.strength, blend);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsNear(Vector3 pos)
    {
        var cols =
        colliderList.FindAll(x => x.gameObject.activeSelf).
        OrderBy(x => Vector3.Distance(x.transform.position, pos));

        if (cols.ToList().Count < 2)
            return false;

        return (Vector3.Distance(pos, cols.ToList()[1].transform.position) < radius * 0.5f);
    }

    public void ResetData()
    {
        while (eraseQueue.Count > 0)
        {
            StartCoroutine(DryCoroutine(eraseQueue.Dequeue()));
        }

        colliderList.ForEach(x => x.gameObject.SetActive(false));
    }
}
