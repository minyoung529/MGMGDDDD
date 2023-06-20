using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField]
    private Chapter chapter;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool isCollide = true;

    [SerializeField]
    private bool playOnAwake = false;

    private PlayableDirector director;

    [SerializeField]
    private bool isOnce = true;
    private bool hasPlayed = false;

    [SerializeField]
    private UnityEvent onCutsceneComplete;


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
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        yield return null;
        CutSceneManager.Instance.Play(director, onCutsceneComplete.Invoke);
    }

    public void SetHasplayed(bool played)
    {
        hasPlayed = played;
    }
}
