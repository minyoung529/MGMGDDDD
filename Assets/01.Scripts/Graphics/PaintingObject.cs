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
    private OilRoot triggerRoots;

    private const int OIL_MAX_SIZE = 100;

    private float distanceChecker = 0f;
    private Vector3 prevPosition = Vector3.zero;
    PaintStructure[] paintLogs = new PaintStructure[OIL_MAX_SIZE];

    [SerializeField]
    private Fire oilTriggerPrefab;
    private List<Fire> oilList = new List<Fire>();

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

    private WaitForSeconds fireDelay = new WaitForSeconds(0.03f);

    new private SphereCollider collider;

    [field: SerializeField]
    public UnityEvent<float> OnSpreadOil { get; set; }
    [field: SerializeField]
    public UnityEvent OnResetOil { get; set; }

    private void Start()
    {
        prevPosition = transform.position;
        collider = GetComponent<SphereCollider>();
        triggerRoots.transform.SetParent(null);

        triggerRoots.OnContactFirePet += Burn;

        for (int i = 0; i < OIL_MAX_SIZE; i++)
        {
            Fire fire = Instantiate(oilTriggerPrefab, triggerRoots.transform);
            oilList.Add(fire);
            triggerRoots.AddFire(fire);

            fire.gameObject.SetActive(false);

            float perlinNoise = Mathf.PerlinNoise(0, i * 20f / (float)OIL_MAX_SIZE);
            perlinNoise = Mathf.Lerp(0.5f, 1f, perlinNoise);
            fire.transform.localScale = Vector3.one * radius * 1.5f * perlinNoise;
        }
    }

    private void Update()
    {
        if (!isPainting) return;
        UpdateSkill(transform.position);
    }

    private void UpdateSkill( Vector3 contact)
    {
        if (distanceChecker > eraseDistance)
        {
            CreateOilPaint(contact);
            curIdx++;
            distanceChecker = 0f;
        }

        distanceChecker += Vector3.Distance(prevPosition, transform.position);
        prevPosition = transform.position;
    }

    void CreateOilPaint(Vector3 point)
    {
        if (curIdx == oilList.Count) return;
        Transform oilTransform = oilList[curIdx].transform;
        oilTransform.position = point;
        oilTransform.gameObject.SetActive(true);
    }

    public void ResetData()
    {
        triggerRoots.OnDryOil?.Invoke();

        OnResetOil?.Invoke();
        curIdx = 0;
        triggerRoots.ResetAllOil();
        isBurning = false;
    }

    private void Burn(object sender, EventArgs eventArgs)
    {
        if (isBurning) return;
        isBurning = true;

        int index = oilList.IndexOf((Fire)sender);
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

        yield return new WaitForSeconds(triggerRoots.BurnDuration);
        isBurning = false;
    }
}
