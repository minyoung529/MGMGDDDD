using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] bool isCheckPoint;

    public bool IsChapterPoint { get { return isCheckPoint; } }

    [SerializeField] private Chapter chapter;
    public Chapter Chapter { get { return chapter; } }
}
