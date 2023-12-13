using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HelpButton : UI_Base
{
    [SerializeField] private Button _helpButton;

    public void AddButtonAction(Action action)
    {
        _helpButton.onClick.AddListener(() => action()) ;
    }

    public void RemoveButtonAction()
    {
        _helpButton.onClick.RemoveAllListeners();
    }

    public override void OpenUI(bool isSound = false, bool isAnimated = false)
    {
        base.OpenUI(false, false);
    }

    public override void CloseUI(bool isSound = false, bool isAnimated = false)
    {
        base.CloseUI(false, false);
    }
}
