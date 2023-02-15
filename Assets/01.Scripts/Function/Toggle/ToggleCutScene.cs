using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCutScene : MonoBehaviour
{
    [SerializeField] private string cutSceneName;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool isCollide = true;

    private bool isPlay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollide)
        {
            if (((1 << other.gameObject.layer) & layerMask) != 0)
                Trigger();
        }
    }

    public void Trigger()
    {
        if (isPlay) return;

        isPlay = true;
        GameManager.Instance.CutSceneManager.Play(cutSceneName);
    }
}
