using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButton : MonoBehaviour
{
    [SerializeField] TogglePosition[] wallToggle;
    [SerializeField] Transform undoPosition;

    private ButtonObject button;

    private void Awake()
    {
        button = GetComponent<ButtonObject>();
    }

    public void DoButton()
    {
        ButtonAction();
        MazePuzzleController.Instance.DoButton(this);
    }

    public void Undo()
    {
        if (button.EnterObject != null)
        {
            button.EnterObject.transform.DOMove(undoPosition.position, 0.1f).OnComplete(() =>
            {
                button.EnterObject.GetComponent<Pet>().ResetNav();
                button.PressButton(false);
            });
        }
    }

    public void ButtonAction()
    {
        for (int i = 0; i < wallToggle.Length; i++)
        {
            wallToggle[i].Trigger();
        }
    }

}
