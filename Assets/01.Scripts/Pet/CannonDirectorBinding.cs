using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CannonDirectorBinding : MonoBehaviour
{
    private PlayableDirector director;

    [SerializeField] private GameObject[] pets;
    [SerializeField] private CheckPetType check;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        PetEmotionTrackBinding();
    }

    public void PetEmotionTrackBinding()
    {
        BindingTimeline.Binding(director, "PetEmotionTrack", pets[(int)check.GetInputPet].GetComponent<PetEmotion>());
        BindingTimeline.Binding(director, "PetActivation", pets[(int)check.GetInputPet].gameObject);
        BindingTimeline.Binding(director, "PetTransformTrack", pets[(int)check.GetInputPet].transform);
    }
}
