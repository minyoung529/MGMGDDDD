using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text pwText;

    public void OnSelectNumber(int number)
    {
        pwText.text = number.ToString();
    }

    public void ResetText()
    {
        pwText.text = "*";
    }
}
