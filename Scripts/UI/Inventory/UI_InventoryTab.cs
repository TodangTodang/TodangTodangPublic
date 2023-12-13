using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryTab : UI_Base
{
    public event Action<Enums.InventoryType> OnClicked;

    public Enums.InventoryType type;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Image _tabImage;

    public void SetTabSelectedState(bool IsSelected)
    {
        if (IsSelected) _tabImage.sprite = _selectedSprite;
        else _tabImage.sprite = _unselectedSprite;
    }

    public void CallOnTabClicked()
    {
        OnClicked?.Invoke(type);
        SetTabSelectedState(true);
    }

    public void PlayClickSound()
    {
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON); 
    }
}
