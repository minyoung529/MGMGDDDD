using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour, PlayerCompo {
    private PlayerController controller = null;
    PlayerController PlayerCompo.Controller { get => controller; set => controller = value; }

    [SerializeField] private int maxHp;
    private int curHp;

    private int hash_tDamaged = Animator.StringToHash("tDamaged");
    private bool isInvincible = false;

    private void OnCollisionEnter(Collision collision) {
        if (isInvincible) return;
        if (collision.transform.CompareTag("EnemyAttack")) {
            curHp--;
            anim.SetTrigger(hash_tDamaged);
            StartCoroutine(Invincible(2f));
            if(curHp <= 0) {
                EventManager.TriggerEvent(EventName.PlayerDie);
            }
        }
    }

    private IEnumerator Invincible(float time) {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }
}
