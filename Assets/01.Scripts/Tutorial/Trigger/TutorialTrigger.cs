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
    private bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide || isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            isEnter = true;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            Debug.Log(gameObject.name + " : " + other.gameObject.name);
            tutorialController?.StartTutorial(tutorialType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoEnd || !isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            int len = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, layerMask).Length;
            if (len != 0) return;

            Debug.Log("EXIT");

            isEnter = false;
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
