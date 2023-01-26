using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Data;

public class TestProjectile : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject startPosition;
    [SerializeField] GameObject endPosition;


    IEnumerator LaunchProjectile()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);

            GameObject bullet = Instantiate(projectile, startPosition.transform.position, Quaternion.identity);
            bullet.transform.DOMoveX(endPosition.transform.position.x, 3f);
        }
    }

    private void Start()
    {
        StartCoroutine(LaunchProjectile());
    }
}
