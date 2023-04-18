using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ToggleRotation leftDoor;
    [SerializeField] private ToggleRotation rightDoor;

    private int floor = 1;
    private const int maxFloor = 3;

    private float distance = 18f;
    private float duration = 2f;

    private bool isMoving = false;

    SkinnedMeshRenderer meshRenderer;
    MeshCollider collider;

    private void Awake()
    {
        collider= GetComponent<MeshCollider>();
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    [ContextMenu("Open")]
    public void OpenElevator()
    {
        leftDoor.Open();
        rightDoor.Open();
        UpdateCollider();
    }
    [ContextMenu("Close")]
    public void CloseElevator()
    {
        leftDoor.Close();
        rightDoor.Close();
        UpdateCollider();
    }

    public void UpElevator()
    {
        if (floor >= 3) return;
        floor++;
        isMoving = true;
        CloseElevator();
        transform.DOKill();
        transform.DOMoveY(transform.position.y + distance, duration).OnComplete(() =>
        {
            OpenElevator();
            isMoving = false; 
        });
    }
    public void DownElevator()
    {
        if (floor <= 1) return;
        floor--;
        isMoving = true;
        CloseElevator();
        transform.DOKill();
        transform.DOMoveY(transform.position.y + (distance*-1), duration).OnComplete(() =>
        {
            OpenElevator();
            isMoving = false;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Define.PLAYER_LAYER)
        {
            UpElevator();
        }
    }

    private void UpdateCollider()
    {
        Mesh bakedMesh = new Mesh();
        collider.sharedMesh = bakedMesh;

        meshRenderer.SetBlendShapeWeight(0, 1);

        meshRenderer.BakeMesh(bakedMesh);
        collider.sharedMesh = bakedMesh;
    }
}
