using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PasswordController : MonoBehaviour
{
    [SerializeField]
    private float scale = 1f;

    [SerializeField]
    private float interval = 1f;

    [SerializeField]
    private PasswordButton[] passwords;

    [SerializeField]
    private PasswordScreen[] passwordScreens;

    private int curIdx = 0;
    private const int MAX_PW_COUNT = 4;
    private const string ANSWER = "4317";

    private bool isSuccess = false;
    private bool canInput = true;
    private string curAnswer = "";

    [SerializeField]
    private UnityEvent onClear;

    private void Start()
    {
        foreach (PasswordButton button in passwords)
        {
            button.OnSelectNumber += OnSelectButton;
            button.CanInput += CanButtonInput;
        }
    }


    private void OnValidate()
    {
        for (int i = 0; i < passwords.Length; i++)
        {
            PasswordButton password = passwords[i];
            password.transform.localScale = Vector3.one * scale;
            password.transform.localPosition = Vector3.right * i * interval;
        }
    }

    public void OnSelectButton(int number)
    {
        if (!canInput) return;
        if (isSuccess) return;

        passwordScreens[curIdx++].OnSelectNumber(number);
        curAnswer += number.ToString();

        if (curIdx == MAX_PW_COUNT)     // ÆÇ´Ü
        {
            if (curAnswer == ANSWER)     // SUCCESS
            {
                isSuccess = true;
                onClear?.Invoke();
            }
            else                        // FAIL
            {
                StartCoroutine(FailCoroutine());
            }

            curIdx = 0;
        }
    }

    private IEnumerator FailCoroutine()
    {
        canInput = false;

        foreach (PasswordButton button in passwords)
            button.Fail();

        yield return new WaitForSeconds(1.7f);

        foreach (PasswordButton button in passwords)
            button.ResetButton();

        foreach (PasswordScreen screen in passwordScreens)
            screen.ResetText();

        curIdx = 0;
        curAnswer = string.Empty;
        canInput = true;
    }

    private bool CanButtonInput()
    {
        return !isSuccess && canInput;
    }
}
