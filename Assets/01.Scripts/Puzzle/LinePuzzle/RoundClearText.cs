using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoundClearText : MonoBehaviour
{
    public void Clear()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * 0.7f;
        transform.DOScale(1.5f, 2f).OnComplete(() => gameObject.SetActive(false));
    }
}
