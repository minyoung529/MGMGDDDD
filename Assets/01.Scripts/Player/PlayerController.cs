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
    private ThirdPersonCameraControll cameraController;

    public Rigidbody Rigid => rigid;
    public Animator Anim => anim;
    public Collider Coll => coll;
    public PlayerMove Move => move;
    public PlayerHP HP => hp;
    public PlayerRespawn Respawn => respawn;
    public PlayerHold Hold => hold;
    public ThirdPersonCameraControll Camera => cameraController;
    #endregion

    private Dictionary<string, PlayerMono> playerMonoDictionary = new Dictionary<string, PlayerMono>();
    /// <summary>
    /// �ҷ������� Ŭ������ �̸����� ������ �� �ֽ��ϴ�.
    /// </summary>
    public Dictionary<string, PlayerMono> PlayerMonoDictionary => playerMonoDictionary;


    private void Awake() {
        SetUpCompo();
        RegisterMono();

        move = GetComponent<PlayerMove>();
        hp = GetComponent<PlayerHP>();
        respawn = GetComponent<PlayerRespawn>();
        hold = GetComponent<PlayerHold>();
        cameraController = GetComponent<ThirdPersonCameraControll>();
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

    public void ResetPlayerSetting()
    {
     //   Move.ChangeState(PlayerStateName.DefaultMove);
        Hold.ResetHold();
       // cameraController.SetDefault();
    }

}
