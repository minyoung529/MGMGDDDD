using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerToEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent action;

    public void Invoke(float time) {
        StartCoroutine(Timer(time));
    }

    private IEnumerator Timer(float time) {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}
