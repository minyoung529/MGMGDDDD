using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region ���Ǽ� ����, ����
    private Rigidbody rigid;
    private Animator anim;
    private Collider coll;
    private PlayerMove move;
    private PlayerHP hp;
    private PlayerRespawn respawn;
    private PlayerHold hold;

    public Rigidbody Rigid => rigid;
    public Animator Anim => anim;
    public Collider Coll => coll;
    public PlayerMove Move => move;
    public PlayerHP HP => hp;
    public PlayerRespawn Respawn => respawn;
    public PlayerHold Hold => hold;
    #endregion

    private Dictionary<string, PlayerMono> playerMonoDictionary = new Dictionary<string, PlayerMono>();
    /// <summary>
    /// �ҷ������� Ŭ������ �̸����� ������ �� �ֽ��ϴ�.
    /// </summary>
    public Dictionary<string, PlayerMono> PlayerMonoDictionary => playerMonoDictionary;


    private void Awake() {
        SetUpCompo();
        RegisterMono();
    }

    private void SetUpCompo() {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }

    private void RegisterMono() {
        PlayerMono.SetController(this);
        PlayerMono[] monoes = GetComponentsInChildren<PlayerMono>();
        foreach (PlayerMono item in monoes) {
            playerMonoDictionary.Add(item.GetType().ToString(), item);
        }
    }
}
