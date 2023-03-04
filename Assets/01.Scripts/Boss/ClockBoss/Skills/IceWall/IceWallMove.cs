using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallMove : MonoBehaviour
{
    private Vector3 size;
    private float spawnTime;
    private float liveTime;
    private float speed;

    public void SpawnWall(Vector3 size, float spawnTime, float liveTime, float speed) {
        this.size = size;
        this.spawnTime = spawnTime;
        this.liveTime = liveTime;
        this.speed = speed;
        StartCoroutine(SetupSize());
        StartCoroutine(Move());
    }

    private IEnumerator SetupSize() {
        float time = 0;
        while(time < spawnTime) {
            transform.localScale = Vector3.Lerp(transform.localScale, size, time / spawnTime);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Move() {
        float time = 0;
        while(time < liveTime) {
            transform.position += transform.forward * speed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(DestroyCoroutine());
    }

    public void Destroy() {
        StopAllCoroutines();
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine() {
        float time = 0;
        while (time < spawnTime) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, time / spawnTime);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
