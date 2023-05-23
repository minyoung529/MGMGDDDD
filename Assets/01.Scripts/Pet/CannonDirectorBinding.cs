using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CannonDirectorBinding : MonoBehaviour
{
    [SerializeField] private PlayableDirector failDirector;
    [SerializeField] private PlayableDirector correctDirector;

    [SerializeField] private GameObject[] pets;
    [SerializeField] private CheckPetType check;

    public void PetEmotionTrackBinding()
    {
        DirectorTrackBinding.Binding(failDirector, "PetEmotionTrack", pets[(int)check.GetInputPet].GetComponent<PetEmotion>());
        DirectorTrackBinding.Binding(failDirector, "PetActivation", pets[(int)check.GetInputPet].gameObject);
        DirectorTrackBinding.Binding(failDirector, "PetTransformTrack", pets[(int)check.GetInputPet].transform);
    }

    public void PetStickyBinding()
    {
        Debug.Log(PetManager.Instance.GetPetByKind<StickyPet>().name);
        DirectorTrackBinding.Binding(correctDirector, "PetTransformTrack", PetManager.Instance.GetPetByKind<StickyPet>().transform);
    }
}
