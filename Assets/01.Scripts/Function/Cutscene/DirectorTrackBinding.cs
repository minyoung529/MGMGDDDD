using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorTrackBinding : MonoBehaviour
{

    public static void Binding(PlayableDirector director, string trackName, Object component)
    {
        BindingTimeline.Binding(director, trackName, component);
    }
}
