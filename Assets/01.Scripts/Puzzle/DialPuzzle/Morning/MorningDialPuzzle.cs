using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;


public class MorningDialPuzzle : MonoBehaviour
{
    [SerializeField] List<GameObject> carPrefabs = new List<GameObject>();

    private List<GameObject> cars = new List<GameObject>();

    private float minDriveTime = 1.5f;
    private float maxDriveTime = 4.5f;

    private void Start()
    {
        StartPuzzle();
    }

    public void StartPuzzle()
    {
        GetCar();
    }
    
    public void StopPuzzle()
    {
    }

    private GameObject InstantiateCar()
    {
        int carType = Random.Range(0, carPrefabs.Count);
        GameObject car = Instantiate(carPrefabs[carType]);
        car.transform.SetParent(transform);
        cars.Add(car);

        car.SetActive(true);

        return car;
    }

    public void GetCar()
    {
        int randomCar = Random.Range(0, cars.Count);
        Transform car;
        if (cars.Count == 0)
        {
            car = InstantiateCar().transform;
        }
        else
        {
            car = cars[randomCar].transform;
        }
        
        cars.Remove(car.gameObject);
        car.transform.localPosition = Vector3.zero;
        car.gameObject.SetActive(true);
        car.DOLocalMoveZ(0.3f, Random.Range(minDriveTime, maxDriveTime)).SetEase(Ease.Flash).OnComplete(() =>
        {
            ReturnCar(car.gameObject);
            GetCar();
        });

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

    // 예외
    // 스폰 시 ::
    // 일정 거리 이하면 앞에 차보다 느려야 함 뚫고 추월X
    // 방금 스폰된 곳이 아니어야 함
    // 여러 곳에 동시에 출발 가능
}
