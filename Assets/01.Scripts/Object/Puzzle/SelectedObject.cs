using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Color outlineColor = Color.blue;

    public bool IsInteraction { get { return interactionObj != null; } }
    public LayerMask InterationLayer { get { return layerMask; } }
    public GameObject InteractiveObj { get { return interactionObj.gameObject; } }
    private OutlineScript interactionObj = null;

    private void Awake()
    {
    }

    public void CheckObject()
    {
        RaycastHit hit;
        Ray ray = GameManager.Instance.MainCam.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            OutlineScript selected = hit.collider.GetComponent<OutlineScript>();

            if (selected != null)
            {
                interactionObj = selected;
                interactionObj.SetColor(outlineColor);
                interactionObj.OnOutline();
            }
            else
            {
                if (interactionObj != null)
                {
                    interactionObj.OffOutline();
                    interactionObj = null;
                }
            }
        }
        else
        {
            if (interactionObj != null)
            {
                interactionObj.OffOutline();
                interactionObj = null;
            }
        }
    }
}