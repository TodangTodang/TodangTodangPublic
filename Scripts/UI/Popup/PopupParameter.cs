using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_Popup;


public class PopupParameter 
{
    protected string _content;
    protected Callback _confirmCallback;
    protected Callback _cancelCallback;

    public PopupParameter(string content = "", Callback confirmCallback = null, Callback cancelCallback = null)
    {
        _content = content;
        _confirmCallback = confirmCallback;
        _cancelCallback = cancelCallback;
    }

    public string GetContent()
    {
        return _content;
    }

    public Callback GetConfirmCallback()
    {
        return _confirmCallback;
    }

    public Callback GetCancelCallback()
    {
        return _cancelCallback;
    }
}
