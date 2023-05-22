using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private UnityEvent OnRespawn;

    private bool canRespawn = false;

    private Collider col;
    private Rigidbody rigid;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        if (!canRespawn) return;
        OnRespawn?.Invoke();
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        col.enabled = false;
        transform.position = respawnPoint.position;
        //while (!Physics.Raycast(transform.position, Vector3.down, 1f, 1 << Define.BOTTOM_LAYER))
        //{
        //    yield return null;
        //}
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        col.enabled = true;
        yield return null;
    }

    public void OnInstalled(Transform transform)
    {
        if (this.transform == transform)
        {
            canRespawn = true;
        }
    }

    public void SetRespawnable(bool value) {
        canRespawn = value;
    }
}
