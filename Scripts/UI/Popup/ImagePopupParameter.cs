using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_Popup;

public class ImagePopupParameter : PopupParameter
{
    private Sprite[] _spriteList;

    public ImagePopupParameter(Sprite[] spriteList, string content = "", Callback confirmCallback = null, Callback cancelCallback = null) 
        : base(content, confirmCallback, cancelCallback)
    {
        _spriteList = spriteList;
    }

    public Sprite[] GetSpriteList()
    {
        return _spriteList;
    }
    
}
