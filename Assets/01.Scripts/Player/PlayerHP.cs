using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int curHp;
    [SerializeField] private UnityEvent onDie;

    private Animator anim;
    private int hash_tDamaged = Animator.StringToHash("tDamaged");
    private bool isInvincible = false;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (isInvincible) return;
        if (collision.transform.CompareTag("EnemeyAttack")) {
            curHp--;
            anim.SetTrigger(hash_tDamaged);
            StartCoroutine(Invincible(0.2f));
            if(curHp <= 0) {
                onDie?.Invoke();
            }
        }
    }

    private IEnumerator Invincible(float time) {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }
}
