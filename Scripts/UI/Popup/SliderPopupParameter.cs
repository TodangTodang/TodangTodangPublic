using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_Popup;

public class SliderPopupParameter : PopupParameter
{
    private int _sliderMaxValue;
    private Action<int> _valueConfirmAction; 

    public SliderPopupParameter(int sliderMaxValue, Action<int> valueConfirmAction, 
        string content = "", Callback confirmCallback = null, Callback cancelCallback = null)
    : base(content, confirmCallback, cancelCallback)
    {
        _sliderMaxValue = sliderMaxValue;
        _valueConfirmAction = valueConfirmAction;
    }

    public int GetSliderMaxValue()
    {
        return _sliderMaxValue;
    }

    public Action<int> GetValueConfirmAction()
    {
        return _valueConfirmAction;
    }
}
