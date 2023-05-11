using UnityEngine;

public abstract class PetState : MonoBehaviour, IState {
    protected Pet pet;
    public abstract PetStateName StateName { get; }

    public virtual void SetUp(Transform root) {
        pet = root.GetComponent<Pet>();
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}