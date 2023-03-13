using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class IceWallSpawner : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float distance;

    private List<IceWallMove> list = new List<IceWallMove>();

    public void StartSpawn(SpawnData data, float speed, Action onComplete = null) {
        StartCoroutine(SpawnCoroutine(data, speed, onComplete));
    }

    private IEnumerator SpawnCoroutine(SpawnData data, float speed, Action onComplete = null) {
        for (int i = 0; i < data.spawnCount; i++) {
            IceWallMove obj = Instantiate(data.prefab, transform).GetComponent<IceWallMove>();
            list.Add(obj);
            obj.transform.forward = transform.forward;
            obj.transform.position += Vector3.Cross(transform.forward, Vector3.up) * Random.Range(-(int)range, (int)range);
            obj.Move(data.spawnSize, data.spawnTime, distance, speed);
            yield return new WaitForSeconds(data.spawnTerm);
        }
        yield return new WaitForSeconds(2f);
        onComplete?.Invoke();
    }

    public void StopSpawn() {
        StopAllCoroutines();
        foreach (IceWallMove item in list)
            item.Destroy();
        list.Clear();
    }
}
