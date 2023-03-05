using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaySequenceCutscene : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector[] directors;

    private void Start()
    {
        StartCoroutine(PlayAllDirectors());
    }

    private IEnumerator PlayAllDirectors()
    {
        for (int i = 0; i < directors.Length; i++)
        {
            directors[i].gameObject.SetActive(true);
            directors[i].Play();

            yield return new WaitForSeconds((float)directors[i].duration);
            directors[i].gameObject.SetActive(false);
        }
    }
}
