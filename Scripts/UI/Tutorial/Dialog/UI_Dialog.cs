using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : UI_Base
{
    public event Action OnClicked;

    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _carrotNextButton;
    [SerializeField] private GameObject _background;

    private void Awake()
    {
        #region Check Null SerializeField
        Debug.Assert(_dialogText != null, "Null Exception : _dialogText");
        Debug.Assert(_nextButton != null, "Null Exception : _nextButton");
        Debug.Assert(_carrotNextButton != null, "Null Exception : _carrotNextButton");
        Debug.Assert(_background != null, "Null Exception : _background");
        #endregion

        _nextButton.onClick.AddListener(CallOnClicked);
        _carrotNextButton.onClick.AddListener(CallOnClicked);
    }

    public void SetDialogText(string text)
    {
        _dialogText.text = text;
    }

    public void SetNextButton(bool isActive)
    {
        _carrotNextButton.gameObject.SetActive(isActive);
    }

    private void CallOnClicked()
    {
        OnClicked?.Invoke();
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null) soundManager.Play(Strings.Sounds.UI_BUTTON);
    }
}
