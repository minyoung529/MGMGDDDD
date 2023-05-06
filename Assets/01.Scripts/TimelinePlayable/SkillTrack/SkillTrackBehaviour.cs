using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SkillTrackBehaviour : PlayableBehaviour
{
    [HideInInspector]    public Pet pet;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        if (pet == null) return;

        pet.Skill();
    }

}
