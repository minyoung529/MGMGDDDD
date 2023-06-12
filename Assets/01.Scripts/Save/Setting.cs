using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : MonoSingleton<Setting>
{
    [SerializeField]
    private Slider hSensitivity;
    [SerializeField]
    private Text hSensitivityText;

    [SerializeField]
    private Slider vSensitivity;
    [SerializeField]
    private Text vSensitivityText;

    [SerializeField]
    private Slider masterVolume;

    [SerializeField]
    private Slider bgmVolume;

    [SerializeField]
    private Slider sfxVolume;

    private CanvasGroup canvasGroup;

    private bool isActive = false;

    private SaveData saveData;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        InputManager.StartListeningInput(InputAction.Escape, ToggleActive);
        Setting.Instance.gameObject.SetActive(false);   // To set instance

        hSensitivity.onValueChanged.AddListener(OnUpdatevalue);
        vSensitivity.onValueChanged.AddListener(OnUpdatevalue);
        masterVolume.onValueChanged.AddListener(OnUpdatevalue);
        bgmVolume.onValueChanged.AddListener(OnUpdatevalue);
        sfxVolume.onValueChanged.AddListener(OnUpdatevalue);
    }

    public void Active()
    {
        LoadValue();
        gameObject.SetActive(true);
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.3f);
        isActive = true;
    }

    public void Inactive()
    {
        SaveData();
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, 0.3f).OnComplete(() => gameObject.SetActive(false));
        isActive = false;
    }

    public void ToggleActive(InputAction action = InputAction.Escape, float value = 0f)
    {
        if (isActive)
        {
            Inactive();
        }
        else
        {
            Active();
        }
    }

    private void LoadValue()
    {
        saveData = SaveSystem.CurSaveData;

        hSensitivity.value = saveData.hSensitivity;
        vSensitivity.value = saveData.vSensitivity;
        masterVolume.value = saveData.masterVolume;
        bgmVolume.value = saveData.bgmVolume;
        sfxVolume.value = saveData.sfxVolume;
    }

    private void SaveData(float value = 0f)
    {
        SaveSystem.CurSaveData.hSensitivity = hSensitivity.value;
        SaveSystem.CurSaveData.vSensitivity = vSensitivity.value;
        SaveSystem.CurSaveData.masterVolume = masterVolume.value;
        SaveSystem.CurSaveData.bgmVolume = bgmVolume.value;
        SaveSystem.CurSaveData.sfxVolume = sfxVolume.value;
    }

    private void OnUpdatevalue(float value)
    {
        hSensitivityText.text = hSensitivity.value.ToString("0.00");
        vSensitivityText.text = vSensitivity.value.ToString("0.00");

        SoundManager.Instance.SetVolume("Master", masterVolume.value);
        SoundManager.Instance.SetVolume("SFX", sfxVolume.value);
        SoundManager.Instance.SetVolume("BGM", bgmVolume.value);
    }

    private void OnDestroy()
    {
        SaveSystem.Save(SaveSystem.CurSaveData);
    }
}