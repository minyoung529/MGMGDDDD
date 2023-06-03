using UnityEngine;

public class PauseState : PetState {
    public override PetStateName StateName => PetStateName.Pause;
    private Vector3 velocity = Vector3.zero;

    public override void OnEnter() {
        CutSceneManager.Instance.AddEndCutscene(OnEndCutScene);
        pet.SetTargetNull();
        pet.SetNavIsStopped(true);
        pet.Rigid.isKinematic = true;
        pet.Rigid.useGravity = false;
        velocity = pet.Rigid.velocity;
        pet.Rigid.velocity = Vector3.zero;
    }

    public override void OnExit() {
        CutSceneManager.Instance.RemoveEndCutscene(OnEndCutScene);
        pet.SetNavIsStopped(false);
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;
        pet.Rigid.velocity = velocity;
    }

    public override void OnUpdate() {

    }

    public void OnEndCutScene() {
        pet.State.ChangeState(pet.State.BeforeStateIndex);
    }
}