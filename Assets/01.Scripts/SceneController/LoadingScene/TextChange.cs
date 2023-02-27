using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    [SerializeField]
    private Text loadingText;

    [SerializeField]
    private string[] loadingTexts;
    [SerializeField]
    private float loadingInterval = 0.5f;

    private int textIdx = 0;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > loadingInterval)
        {
            textIdx = (textIdx + 1) % loadingTexts.Length;
            loadingText.text = loadingTexts[textIdx];
            timer = 0f;
        }
    }
}
