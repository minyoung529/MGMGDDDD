using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UIElements;
using UnityEngine.Events;

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
    private Transform triggerRoots;

    private const int OIL_MAX_SIZE = 100;

    private float distanceChecker = 0f;
    private Vector3 prevPosition = Vector3.zero;
    PaintStructure[] paintLogs = new PaintStructure[OIL_MAX_SIZE];

    [SerializeField]
    private PaintedOil oilTriggerPrefab;
    private List<PaintedOil> oilList = new List<PaintedOil>();

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

    private bool isBurning = false;
    private int curIdx = 0;

    private WaitForSeconds fireDelay = new WaitForSeconds(0.08f);

    new private SphereCollider collider;

    [field: SerializeField]
    public UnityEvent<float> OnSpreadOil { get; set; }
    [field: SerializeField]
    public UnityEvent OnResetOil { get; set; }

    private void Start()
    {
        prevPosition = transform.position;
        collider = GetComponent<SphereCollider>();
        triggerRoots.SetParent(null);

        for (int i = 0; i < OIL_MAX_SIZE; i++)
        {
            PaintedOil oil = Instantiate(oilTriggerPrefab, triggerRoots);
            oilList.Add(oil);

            oil.OnContactFirePet += Burn;
            oil.gameObject.SetActive(false);
            oil.transform.localScale = Vector3.one * radius * 1.5f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isPainting) return;
        if (((1 << collision.gameObject.layer) & layerMask) == 0) return;
        if (curIdx >= OIL_MAX_SIZE) return;

        UpdateSkill(collision.gameObject, collision.contacts[0].point);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPainting) return;
        if (((1 << other.gameObject.layer) & layerMask) == 0) return;
        if (curIdx >= OIL_MAX_SIZE) return;


        UpdateSkill(other.gameObject, other.ClosestPoint(transform.position + collider.center));
    }

    private void UpdateSkill(GameObject obj, Vector3 contact)
    {
        Paintable p = obj.GetComponent<Paintable>();

        if (!p) return;

        if (distanceChecker > eraseDistance)
        {
            if (IsNear(contact)) return;

            CreateOilPaint(contact, p);

            PaintStructure paint = new();
            paint.DataSet(p, contact, radius, 0.2f, 1f, color);

            paintLogs[curIdx++] = paint;
            distanceChecker = 0f;
        }

        distanceChecker += Vector3.Distance(prevPosition, transform.position);
        prevPosition = transform.position;
    }

    void CreateOilPaint(Vector3 point, Paintable p)
    {
        PaintedOil oil = oilList[curIdx];
        oil.transform.position = point;
        oil.gameObject.SetActive(true);

        PaintStructure paintData = new PaintStructure();
        paintData.DataSet(p, point, radius, 0.2f, 1f, color);

        StartCoroutine(SpreadCoroutine(paintData));
    }

    private IEnumerator DryCoroutine(PaintStructure p)
    {
        float timer = 0f;

        while (timer < OIL_PAINT_DURATION)
        {
            Color blend = Color.Lerp(p.color.Value, Color.clear, timer / OIL_PAINT_DURATION);
            PaintManager.Instance.Paint(p.paint, p.point, p.radius * 0.75f, p.hardness, p.strength, blend);

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
            PaintManager.Instance.Paint(top.paint, top.point, top.radius * 0.75f, top.hardness, top.strength, blend);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsNear(Vector3 pos)
    {
        var cols =
        oilList.FindAll(x => x.gameObject.activeSelf).
        OrderBy(x => Vector3.Distance(x.transform.position, pos));

        if (cols.ToList().Count < 2)
            return false;

        return (Vector3.Distance(pos, cols.ToList()[1].transform.position) < radius * 0.5f);
    }

    public void ResetData()
    {
        for (int i = 0; i < oilList.Count; i++)
        {
            if (oilList[i].gameObject.activeSelf)
            {
                StartCoroutine(DryCoroutine(paintLogs[i]));
            }
        }

        OnResetOil?.Invoke();
        curIdx = 0;
        oilList.ForEach(x => x.ResetOil());
        isBurning = false;
    }

    private void Burn(object sender, EventArgs eventArgs)
    {
        if (isBurning) return;
        isBurning = true;

        int index = oilList.IndexOf((PaintedOil)sender);
        StartCoroutine(BurnCoroutine(index));
    }

    private IEnumerator BurnCoroutine(int index)
    {
        int maxInterval = Mathf.Max(Mathf.Abs(curIdx - index), index);
        oilList[index].Burn();

        for (int i = 1; i <= maxInterval; i++)
        {
            int left = index - i, right = index + i;

            if (left >= 0)
            {
                oilList[left].Burn();
            }
            if (right < curIdx)
            {
                oilList[right].Burn();
            }

            yield return fireDelay;
        }

        yield return new WaitForSeconds(oilList[index].BurnDuration);
    }
}
