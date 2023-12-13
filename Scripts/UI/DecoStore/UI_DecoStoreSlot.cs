using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DecoStoreSlot : MonoBehaviour
{
    public event Action<int> OnClicked;

    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _effectText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;

    private int _slotID;
    private Image _btnImage;
    private Sprite _buyBtnSprite;
    private Sprite _cantBuyBtnSprite;
    private Sprite _soldOutBtnSprite;

    private void Awake()
    {
        _btnImage = _buyButton.GetComponent<Image>();
        _buyBtnSprite = ResourceManager.Instance.LoadSprite(Strings.Sprites.DECO_STORE_BUY_BTN);
        _cantBuyBtnSprite = ResourceManager.Instance.LoadSprite(Strings.Sprites.DECO_STORE_CANT_BUY_BTN);
        _soldOutBtnSprite = ResourceManager.Instance.LoadSprite(Strings.Sprites.DECO_STORE_SOLD_OUT_BTN);
        _buyButton.onClick.AddListener(CallOnClicked);
    }

    public void Init(StoreDecorationInfoData data, int money)
    {
        StoreDecorationInfoSO dataSO = data.DefaultData;
        _itemImage.sprite = dataSO.IconSprite;
        _slotID = dataSO.ID;
        _nameText.text = dataSO.Name;
        _effectText.text = dataSO.Description;
        _priceText.text = $"{dataSO.Price}";

        if (data.IsSoldOut)
        {
            _btnImage.sprite = _soldOutBtnSprite;
            _buyButton.enabled = false;
        }
        else
        {
            if (money >= dataSO.Price)
            {
                _btnImage.sprite = _buyBtnSprite;
                _buyButton.enabled = true;
            }
            else
            {
                _btnImage.sprite = _cantBuyBtnSprite;
                _buyButton.enabled = false;
            }
        }
    }

    public void SetClickAction(Action<int> action)
    {
        OnClicked = null;
        OnClicked += action;
    }

    private void CallOnClicked()
    {
        OnClicked?.Invoke(_slotID);
    }
}
