using UnityEngine;

[System.Serializable]
public struct SpawnData {
    public GameObject prefab; //프리팹
    public Vector3 spawnSize; //크기
    public float spawnTime; //스폰 소요 시간
    public float spawnTerm; //스폰 주기
    public float spawnCount; //갯수
}
