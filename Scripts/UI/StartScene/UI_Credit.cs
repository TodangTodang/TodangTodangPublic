using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Credit : UI_Base
{
    [SerializeField] private Button _closeBtn;
    
    void Awake()
    {
        _closeBtn.onClick.AddListener(() => CloseUI());   
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
    }
    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        base.CloseUI(isSound, true);
    }
}
