using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeButton : MonoBehaviour
{
    [SerializeField] TogglePosition[] wallToggle;
    private NavMeshObstacle[] navMeshObstacles;
    [SerializeField] Transform undoPosition;
    [SerializeField] Light buttonLight;

    private ButtonObject button;

    private void Awake()
    {
        button = GetComponent<ButtonObject>();
        buttonLight.intensity = 0f;

        navMeshObstacles = new NavMeshObstacle[wallToggle.Length];

        for (int i = 0; i < wallToggle.Length; i++)
        {
            navMeshObstacles[i] = wallToggle[i].GetComponent<NavMeshObstacle>();
            navMeshObstacles[i].enabled = wallToggle[i].MoveDir.y < 0f;
        }
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
                button.Press(false);
            });
        }
    }

    public void ButtonAction()
    {
        if (buttonLight.intensity == 0f)
        {
            buttonLight.DOIntensity(7f, 0.3f);
        }
        else
        {
            buttonLight.DOIntensity(0f, 0.3f);
        }

        for (int i = 0; i < wallToggle.Length; i++)
        {
            wallToggle[i].Trigger();
            StartCoroutine(DisableObstacle(i, wallToggle[i].Duration));
        }
    }
    public IEnumerator DisableObstacle(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
            navMeshObstacles[index].enabled = !navMeshObstacles[index].enabled;
    }
}
