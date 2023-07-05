using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider))]
public class SavePoint : MonoBehaviour
{
    [SerializeField] bool isCheckPoint;

    public bool IsChapterPoint { get { return isCheckPoint; } }

    [SerializeField] private Chapter chapter;
    public Chapter Chapter { get { return chapter; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.PLAYER_TAG))
        {
            EventParam eventParam = new EventParam();
            eventParam["SavePoint"] = this;
            EventManager.TriggerEvent(EventName.SavePoint, eventParam);
            Debug.Log("Save");
        }
    }
}
