using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BindingCutscenePet : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject[] cutscenePets;

    public void Binding(string trackName, Object component)
    {
        DirectorTrackBinding.Binding(director, trackName, component);
    }
    
}
