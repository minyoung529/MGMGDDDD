using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordButton : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private int number = 0;

    [SerializeField]
    private TMP_Text text;

    private bool isSelected = false;

    public Action<int> OnSelectNumber { get; set; }
    public Func<bool> CanInput { get; set; }

    [SerializeField] private new MeshRenderer renderer;

    private void OnCollisionEnter(Collision collision)
    {
        if (isSelected) return;
        Debug.Log("Non Select");
        if (CanInput != null && !CanInput.Invoke()) return;
        Debug.Log("Can Input");

        if (((1 << collision.gameObject.layer) & layerMask) != 0) // pet or player
        {
            Debug.Log("Player or Pet");
            isSelected = true;
            SetEmissionColor(new Color(0.03636537f, 0.4150943f, 0f));
            OnSelectNumber?.Invoke(number);
        }
    }

    private void SetEmissionColor(Color color)
    {
        renderer.material.DOKill();
        renderer.material.DOColor(color, "_EmissionColor", 0.3f);
    }

    public void Fail()
    {
        SetEmissionColor(Color.red);
    }

    public void ResetButton()
    {
        isSelected = false;
        SetEmissionColor(Color.white * 0.0196078431f);
    }

    private void OnValidate()
    {
        text.text = number.ToString();
    }
}
