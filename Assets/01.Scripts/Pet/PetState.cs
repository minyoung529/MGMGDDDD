using UnityEngine;

public abstract class PetState : MonoBehaviour, IState {
    protected Pet pet;
    public abstract PetStateName StateName { get; }
    public int fence { get; set; }

    public virtual void SetUp(Transform root) {
        pet = root.GetComponent<Pet>();
        fence = 0;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}