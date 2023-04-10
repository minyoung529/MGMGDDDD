using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;

public enum CarType
{
    BasicCar,
    SportsCar,
    Bus,
    Subway
}
public class MorningDialPuzzle : MonoBehaviour
{
    [SerializeField] Transform carSpawnParent;
    [SerializeField] List<GameObject> carPrefabs = new List<GameObject>();

    private List<GameObject> cars = new List<GameObject>();
    private float[] spawnPositioin = { -5f, -3.32f, 0f, 1.5f, 4.5f, 6f };

    private void Start()
    {
        StartCoroutine(SpawnCar());
    }

    public void StartPuzzle()
    {
    }
    
    public void StopPuzzle()
    {
    }

    private IEnumerator SpawnCar()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            Debug.Log("ASD");
            InstantiateCar();
        }
    }

    private GameObject InstantiateCar()
    {
        int carType = Random.Range(0, carPrefabs.Count);
        GameObject car = Instantiate(carPrefabs[carType]);
        car.transform.SetParent(carSpawnParent);
        cars.Add(car);

        car.SetActive(true);

        return car;
    }
    private void InstantiateCar(CarType type)
    {
        GameObject car = Instantiate(carPrefabs[(int)type]);
        car.transform.SetParent(carSpawnParent);
        car.SetActive(false);
        
        cars.Add(car);
    }

    public void GetCar()
    {
        int randomCar = Random.Range(0, carSpawnParent.childCount);
        Transform car;

        if (carSpawnParent.childCount == 0)
        {
            car = InstantiateCar().transform;
        }
        else
        {
            car = cars[randomCar].transform;
        }
        car.transform.localPosition = new Vector3(spawnPositioin[Random.Range(0, spawnPositioin.Length)], 4.4f, 10f);
        car.gameObject.SetActive(true);

        cars.Remove(car.gameObject);
    }

    public void ReturnCar(GameObject car)
    {
        cars.Add(car);
        car.SetActive(false);
    }

    // 1. 시작
    // 2. 차가 앞으로 지나감
    //    차가 일정 거리 이상 나가면 Return
    // 3. 만약 플레이어랑 부딪히면 GameOver
}
