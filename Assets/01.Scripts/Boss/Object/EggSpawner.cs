using System;
using System.Collections.Generic;
using System.Threading;
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
        list.Remove(easterEgg);

        curCount--;
    }

    private void SpawnEgg()
    { 
        EasterEgg newEgg = Instantiate(prefab, GetRandomPos(out int index), Quaternion.Euler(-90f, 0f, 0f));
        newEgg.SpawnIndex = index;
        newEgg.ListeningOnDestroy(OnEggDestroyed);
        list.Add(newEgg);

        curCount++;
    }

    private Vector3 GetRandomPos(out int randomIndex)
    {
        randomIndex = UnityEngine.Random.Range(0, indexes.Count);
        int spawnIdx = indexes[randomIndex];
        Vector3 pos = spawnPositions[spawnIdx].position;

        indexes.RemoveAt(randomIndex);
        randomIndex = spawnIdx;

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
