using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerLight : MonoBehaviour
{
    [SerializeField]
    private PathFollower follower;

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private bool isToggle = false;
    private bool reverse = false;
    private bool isOn = false;

    [SerializeField]
    private UnityEvent OnArrive;

    private void Start()
    {
        OnArrive.AddListener(Arrive);
        follower.onArrive.AddListener((x) => OnArrive.Invoke());
    }

    public void Trigger(bool isOn)
    {
        if (isToggle && reverse != isOn)
        {
            follower.reverseStartEnd = !follower.reverseStartEnd;
            reverse = isOn;
            this.isOn = isOn;
        }

        Trigger();
    }

    [ContextMenu("Trigger")]
    private void Trigger()
    {
        follower.Depart();
    }

    private void Arrive()
    {
        if (isToggle && !isOn)
        {
            particle.Stop();
        }
    }
}
