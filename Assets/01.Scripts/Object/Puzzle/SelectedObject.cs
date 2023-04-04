using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    OutlineScript selectObj;

    [SerializeField]
    private LayerMask layerMask;

    private void Update()
    {
        CheckObject();
    }

    private void CheckObject()
    {
        RaycastHit hit;
        Ray ray = GameManager.Instance.MainCam.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            OutlineScript selected = hit.collider.GetComponent<OutlineScript>();

            if (selected != null)
            {
                selectObj = selected;
                selectObj.OnOutline();
            }
            else
            {
                if (selectObj != null)
                {
                    selectObj.OffOutline();
                    selectObj = null;
                }
            }
        }
        else
        {
            if (selectObj != null)
            {
                selectObj.OffOutline();
                selectObj = null;
            }
        }
    }
}