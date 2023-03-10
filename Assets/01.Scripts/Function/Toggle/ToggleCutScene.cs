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

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private bool isStart = false;

    private void Start()
    {
        if(isStart)
        {
            Trigger();
        }
    }

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
        CameraSwitcher.ChangeSwitchBlend(2f);
        StartCoroutine(DelayOneFrame());
    }

    private IEnumerator DelayOneFrame()
    {
        yield return null;
        GameManager.Instance.CutSceneManager.Play(cutSceneName, speed);
    }
}
