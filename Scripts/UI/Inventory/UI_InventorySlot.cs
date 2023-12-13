using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : UI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public int ItemIdx { get; set; }

    public event Action<UI_InventorySlot> OnClicked;

    [SerializeField] private Image _slotOutlineImage;
    [SerializeField] private Image _slotItemImage;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _expirationDateText;
    [SerializeField] private GameObject _lockedImage;
    [SerializeField] private GameObject _infoImages;

    private ScrollRect _scrollView;

    public void InitSlotThumbnail(Sprite itemSprite, bool isLocked = false, int quantity = -1, int expirationDate = -1)
    {
        _slotItemImage.sprite = itemSprite;
        if (isLocked)
        {
            _lockedImage.SetActive(true);
            _infoImages.SetActive(false);
        }
        else
        {
            _lockedImage.SetActive(false);

            if (quantity != -1 && expirationDate != -1)
            {
                _infoImages.SetActive(true);
                _quantityText.text = $"{quantity}";
                _expirationDateText.text = $"{expirationDate}";
            } else
            {
                _infoImages.SetActive(false);
            }
        }
    }

    public void UnlockSlot()
    {
        _lockedImage.SetActive(false);
    }

    public void SetSlotSelected(bool isSelected)
    {
        _slotOutlineImage.color = (isSelected) ? Colors.DeepBlue : Colors.LightBlue;
    }

    public void RegisterCallbacks(Action<UI_InventorySlot>[] callbacks)
    {
        OnClicked = null;
        for (int i = 0; i < callbacks.Length; i++) OnClicked += callbacks[i];
    }

    public void CallOnClicked()
    {
        OnClicked?.Invoke(this);
        SetSlotSelected(true);
    }

    public void PlayClickSound()
    {
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);
    }

    public void SetScrollRect(ScrollRect scrollRect)
    {
        _scrollView = scrollRect;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _scrollView.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _scrollView.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollView.OnEndDrag(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        _scrollView.OnScroll(eventData);
    }
}
