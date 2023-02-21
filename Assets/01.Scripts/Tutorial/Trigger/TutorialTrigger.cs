using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private bool isCollide = true;

    [SerializeField]
    private bool autoEnd = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    private TutorialType tutorialType;

    private TutorialController tutorialController;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            tutorialController?.StartTutorial(tutorialType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoEnd) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            tutorialController?.StopTutorial(tutorialType);
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
