using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool isCollide = true;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private bool playOnAwake = false;

    private PlayableDirector director;

    [SerializeField]
    private bool isOnce = true;
    private bool hasPlayed = false;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (playOnAwake)
        {
            Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollide)
        {
            if (((1 << other.gameObject.layer) & layerMask) != 0)
                Play();
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (hasPlayed && isOnce) return;
        
        hasPlayed = true;
        CameraSwitcher.ChangeSwitchBlend(2f);
        StartCoroutine(DelayOneFrame());
    }

    private IEnumerator DelayOneFrame()
    {
        yield return null;
        CutSceneManager.Instance.Play(director, speed);
    }
}
