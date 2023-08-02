using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// 플레이어의 Sound랑 Light를 감지
/// 
/// </summary>
public class BossDetect : MonoBehaviour
{
    private const float maxDetectSound = 5f;
    private const float maxDetectLight = 5f;

    private Boss boss;

    private void Awake()
    {
        boss = GetComponent<Boss>();
    }

    public void DetectSound(GameObject sound)
    {
        SetTarget(sound.transform);
    }
    public void DetectLight(GameObject light)
    {
        SetTarget(light.transform);
    }

    private void SetTarget(Transform target)
    {
        boss.SetItemWaypoint(target);
    }
}
