using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerToEvent : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private UnityEvent action;

    public void Invoke() {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer() {
        yield return new WaitForSeconds(time);
        Debug.Log(1);
        action.Invoke();
    }
}
