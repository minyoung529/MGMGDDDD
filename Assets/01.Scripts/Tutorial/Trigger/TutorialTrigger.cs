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

    [SerializeField]
    private string tutorialName;

    private TutorialController tutorialController = null;
    private bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide || isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            isEnter = true;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();

            Debug.Log($"{other.gameObject.name} Has TC => {tutorialController != null}");

            tutorialController?.StartTutorial(tutorialType, tutorialName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoEnd || !isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            int len = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, layerMask).Length;
            if (len != 0) return;

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
