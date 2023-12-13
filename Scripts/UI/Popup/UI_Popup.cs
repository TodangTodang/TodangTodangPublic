using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    public delegate void Callback();
    private Callback _callbackConfirm;
    private Callback _callbackCancel;

    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        Debug.Assert(_contentText, "Null Exception : _contentText");
        Debug.Assert(_confirmButton, "Null Exception : _confirmButton");
        Debug.Assert(_cancelButton, "Null Exception : _cancelButton");
#endif

        if (_confirmButton != null) _confirmButton.onClick.AddListener(() => ClosePopup(Enums.PopupButtonType.Confirm));
        if (_cancelButton != null) _cancelButton.onClick.AddListener(() => ClosePopup(Enums.PopupButtonType.Cancel));
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        base.CloseUI(isSound, true);
    }

    public virtual void ShowPopup(PopupParameter popupParameter)
    {
        if (popupParameter.GetContent().Length != 0) _contentText.text = popupParameter.GetContent();
        _callbackConfirm = popupParameter.GetConfirmCallback();
        _callbackCancel = popupParameter.GetCancelCallback();

        OpenUI();
    }

    public void ClosePopup(Enums.PopupButtonType type, bool isSound = true)
    {
        if (type == Enums.PopupButtonType.Confirm) _callbackConfirm?.Invoke();
        else _callbackCancel?.Invoke();

        CloseUI(isSound);
    }
}
