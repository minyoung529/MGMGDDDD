using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMoveController : MonoBehaviour
{
    private List<Transform> childs = new List<Transform>();

    [SerializeField]
    private float speed = 1f;

    private float maxDistance = 10f;
    private float curDistance = 0f;

    private bool isActive = false;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            childs.Add(child);
        }

        maxDistance = Vector3.Distance(childs[0].position, childs[1].position);
    }

    private void Update()
    {
        if (!isActive) return;

        foreach (Transform child in childs)
        {
            child.Translate(Vector3.up * speed * Time.deltaTime);
        }

        curDistance += speed * Time.deltaTime;

        if (curDistance > maxDistance)
        {
            ChangeTile();
            curDistance -= maxDistance;
        }
    }

    private void ChangeTile()
    {
        Transform top = childs[0];
        top.position = childs[^1].position - childs[0].up * maxDistance;

        childs.RemoveAt(0);
        childs.Add(top);
    }

    [ContextMenu("ACTIVE")]
    public void SetActivate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    [ContextMenu("INACTIVE")]
    public void SetInactivate()
    {
        isActive = false;
    }
}
