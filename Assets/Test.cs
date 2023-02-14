using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OnDrawGizmos() {

        float maxDistance = 100;
        RaycastHit hit;
        // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        bool isHit = Physics.BoxCast(transform.position + Vector3.up * 0.1f, new Vector3(0.5f, 0, 0.5f), Vector3.down, out hit, Quaternion.identity, 100f, 1 << Define.BOTTOM_LAYER);

        Gizmos.color = Color.red;
        if (isHit) {
            Gizmos.DrawRay(transform.position, hit.point - transform.position);
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, Vector3.one * 0.5f);
        }
        else {
            Gizmos.DrawRay(transform.position, Vector3.down * maxDistance);
        }
    }
}
