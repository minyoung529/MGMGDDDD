using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody Rigid;
    public Animator Anim;
    public Collider Coll;

    private PlayerCompo[] compoList;

    private PlayerMove move;
    private PlayerHP hp;
    private PlayerRespawn respawn;
    private PlayerHold hold;

    private void Awake() {
        SetUpCompo();
        SetUpList();
    }

    private void SetUpCompo() {
        Rigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider>();
    }

    private void SetUpList() {
        compoList = GetComponentsInChildren<PlayerCompo>();
        foreach(PlayerCompo item in compoList) {
            item.SetController(this);
        }
    }
}
