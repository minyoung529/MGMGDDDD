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
        }
    }

    public void Play(string sceneName)
    {
        sceneDictionary[sceneName].Play();
    }
}
