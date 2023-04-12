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

    // 1. ����
    // 2. ���� ������ ������
    //    ���� ���� �Ÿ� �̻� ������ Return
    // 3. ���� �÷��̾�� �ε����� GameOver

    // ����
    // ���� �� ::
    // ���� �Ÿ� ���ϸ� �տ� ������ ������ �� �հ� �߿�X
    // ��� ������ ���� �ƴϾ�� ��
    // ���� ���� ���ÿ� ��� ����
}
