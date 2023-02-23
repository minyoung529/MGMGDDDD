using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    private PlayableDirector[] scenes = null;
    private Dictionary<string, PlayableDirector> sceneDictionary = new Dictionary<string, PlayableDirector>();

    private void Awake()
    {
        scenes = GetComponentsInChildren<PlayableDirector>();
        for (int i = 0; i < scenes.Length; i++)
        {
            sceneDictionary.Add(scenes[i].name, scenes[i]);
            scenes[i].gameObject.SetActive(false);
        }
    }

    public void Play(string sceneName)
    {
        sceneDictionary[sceneName].gameObject.SetActive(true);
        sceneDictionary[sceneName].Play();

        StartCoroutine(WaitForDuration(sceneDictionary[sceneName]));
    }

    private IEnumerator WaitForDuration(PlayableDirector playableDirector)
    {
        yield return new WaitForSeconds((float)playableDirector.duration);
        playableDirector.gameObject.SetActive(false);
    }
}
