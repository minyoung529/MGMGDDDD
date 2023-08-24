using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    [SerializeField]
    private EasterEgg prefab;

    [SerializeField]
    private int maxCount = 5;

    [SerializeField]
    private float spawnDelay;

    private float spawnCounter = 0f;
    private int curCount;

    private List<EasterEgg> list = new();

    [SerializeField]
    private List<Transform> spawnPositions;

    private void Update()
    {
        if (curCount < maxCount)
        {
            spawnCounter += Time.deltaTime;

            if (spawnCounter > spawnDelay)
            {
                SpawnEgg();
                spawnCounter = 0f;
            }
        }
    }

    private void OnEggDestroyed(HoldableObject egg)
    {
        list.Remove(egg as EasterEgg);
        curCount--;
    }

    private void SpawnEgg()
    {
        EasterEgg newEgg = Instantiate(prefab, GetRandomPos(), Quaternion.Euler(-90f, 0f, 0f));
        newEgg.ListeningOnDestroy(OnEggDestroyed);

        list.Add(newEgg);

        curCount++;
    }

    private Vector3 GetRandomPos()
    {
        int fence = 0;

        while (fence < 100)
        {
            int rand = Random.Range(0, spawnPositions.Count);
            float dist = 999f;
            bool tooClose = false;

            if (list.Count > 0)
            {
                foreach (EasterEgg egg in list)
                {
                    dist = Mathf.Min(dist, Vector3.Distance(spawnPositions[rand].position, egg.transform.position));

                    if (dist < 5f)
                        tooClose = true;
                }
            }

            fence++;

            if (tooClose)
                continue;

            else return spawnPositions[rand].position;
        }

        // 비정상적으로 많이 돌았을 때
        Debug.LogError("Egg GetRandomPos");
        return spawnPositions[Random.Range(0, spawnPositions.Count)].position;
    }

    private void OnDestroy()
    {
        foreach (EasterEgg egg in list)
        {
            egg.StopListeningOnDestroy(OnEggDestroyed);
        }
    }
}
