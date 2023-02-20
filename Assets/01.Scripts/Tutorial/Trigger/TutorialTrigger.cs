using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private bool isCollide = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    private TutorialType tutorialType;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            TutorialController controller = other.gameObject.GetComponent<TutorialController>();

            if (controller)
            {
                controller.StartTutorial(tutorialType);
            }
        }
    }

    public void Trigger()
    {
        TutorialController controller = FindObjectOfType<TutorialController>();
        if (!controller) return;

        if (Condition(controller.transform)) // player
        {
            if (controller)
            {
                controller.StartTutorial(tutorialType);
            }
        }
    }

    protected virtual bool Condition(Transform player)
    {
        return true;
    }
}
