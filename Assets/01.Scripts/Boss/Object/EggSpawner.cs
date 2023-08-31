using System;
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

    private List<int> indexes = new List<int>();

    private void Start()
    {
        for (int i = 0; i < maxCount; i++)
        {
            indexes.Add(i);
        }
    }

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
        EasterEgg easterEgg = egg as EasterEgg;
        indexes.Add(easterEgg.SpawnIndex);
        Debug.Log(easterEgg.SpawnIndex);
        list.Remove(easterEgg);

        curCount--;
    }

    private void SpawnEgg()
    {
        EasterEgg newEgg = Instantiate(prefab, GetRandomPos(out int index), Quaternion.Euler(-90f, 0f, 0f));
        newEgg.SpawnIndex = indexes[index];
        newEgg.ListeningOnDestroy(OnEggDestroyed);
        list.Add(newEgg);

        curCount++;

        // RANDOM.RANGE(0, INDEXES.LENGTH) => 1 0 2
        // INDEXES[1] = 0

        // 0을 써줄 거야
        // spawnPosition[0]
        // newEgg.num = 0;
        // indexes.add(0);
    }

    private Vector3 GetRandomPos(out int randomIndex)
    {
        randomIndex = UnityEngine.Random.Range(0, indexes.Count);
        int spawnIdx = indexes[randomIndex];
        Vector3 pos = spawnPositions[spawnIdx].position;

        indexes.RemoveAt(spawnIdx);

        return pos;
    }

    private void OnDestroy()
    {
        foreach (EasterEgg egg in list)
        {
            egg.StopListeningOnDestroy(OnEggDestroyed);
        }
    }
}
