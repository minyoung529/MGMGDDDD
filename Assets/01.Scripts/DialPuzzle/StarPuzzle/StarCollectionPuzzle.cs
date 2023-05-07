using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StarCollectionPuzzle : MonoBehaviour
{
    [SerializeField]
    private int maxPieceCount = 5;

    private int curPieceCount = 0;

    public bool IsSuccess => maxPieceCount == curPieceCount;

    private List<DialStarObject> dialStars = new List<DialStarObject>();

    private bool[] isAttached;

    private bool isFiring = false;

    [SerializeField]
    private List<TorchLight> starRoads;

    private void Awake()
    {
        dialStars = transform.GetComponentsInChildren<DialStarObject>().ToList();
        isAttached = new bool[maxPieceCount];

        ListeningEvent();
    }

    private void ListeningEvent()
    {
        foreach (DialStarObject dialStarObject in dialStars)
        {
            dialStarObject.OnAttached += OnAttachToSticky;
            dialStarObject.OnContactFire += OnContactFire;
        }
    }

    #region LISTENING ACTION
    public void OnAttachToSticky(int index)
    {
        if (isAttached[index]) return;

        curPieceCount++;
        isAttached[index] = true;
    }

    public void OnContactFire(int index)
    {
        if (isFiring) return;
        if (!IsAllAttach()) return;

        isFiring = true;
        Success();
    }
    #endregion

    #region COMMAND
    private void Success()
    {
        dialStars.ForEach(x => x.Success());
        starRoads.ForEach(x => x.OnLight());
    }

    public void ResetStars()
    {
        starRoads.ForEach(x => x.OffLight());
        dialStars.ForEach(x => x.ResetStar());
    }
    #endregion

    private bool IsAllAttach()
    {
        foreach (bool element in isAttached)
        {
            if(!element)
                return false;
        }

        return true;
    }
}
