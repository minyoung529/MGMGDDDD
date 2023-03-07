using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private UnityEvent OnRespawn;

    private Collider col;

    private void Awake() {
        col = GetComponent<Collider>();
    }

    public void Respawn() {
        OnRespawn?.Invoke();
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine() {
        col.enabled = false;    
        transform.position = respawnPoint.position;
        while (!Physics.Raycast(transform.position, Vector3.down, 1f, 1 << Define.BOTTOM_LAYER)) {
            yield return null;
        }
        col.enabled = true;
    }
}
