using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_StoreClosed : UI_Base
{
    public event Action OnUIOpen;

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        UIManager _uiManager = UIManager.Instance;
        UI_Base _ui = _uiManager.GetCurrentUI();
        while (_ui != null)
        {
            if (_ui is UI_Popup) (_ui as UI_Popup).ClosePopup(Enums.PopupButtonType.Cancel);
            else _ui.CloseUI();
            _ui = _uiManager.GetCurrentUI();
        }

        base.OpenUI(isSound, isAnimated);
        OnUIOpen?.Invoke();
        Time.timeScale = 0;
    }

    public override void CloseUI(bool isSound = false, bool isAnimated = true)
    {
        Time.timeScale = 1;
        base.CloseUI(isSound, true);
    }
}
