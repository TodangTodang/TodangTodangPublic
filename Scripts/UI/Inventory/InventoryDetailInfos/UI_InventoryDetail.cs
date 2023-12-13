using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryDetail : UI_Base
{
    #region SerializeField
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _warningText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _upgradeButtonText;

    [SerializeField] private Image _detailItemImage;
    [SerializeField] private GameObject _priceTextBackground;
    [SerializeField] private Button _upgradeButton;

    [SerializeField] private UI_InventoryTypeDatas _uiInventoryTypeDatas;
    [SerializeField] private UI_InventoryAdditionalDatas _uiInventoryAdditionalDatas;
    #endregion

    public event Action OnSellUpgradeBtnClicked;

    private int _star;
    private int _money;

    private void Start()
    {
        _upgradeButton.onClick.AddListener(() => { OnSellUpgradeBtnClicked?.Invoke(); });
    }

    public void SetPlayerInfo(int playerStar, int playerMoney) 
    {
        _star = playerStar;
        _money = playerMoney;
    }

    public void SetButtonClickListener(Action clickListener)
    {
        OnSellUpgradeBtnClicked = null;
        OnSellUpgradeBtnClicked += clickListener;
    }

    public void CallOnSellUpgradeBtnClicked()
    {
        OnSellUpgradeBtnClicked?.Invoke();
    }

    public void SetCommonContent(Sprite sprite, string name, string description)
    {
        _detailItemImage.sprite = sprite;
        _titleText.text = name;
        _descriptionText.text = description;
    }

    public void SetIngredientDetailContent(int quantity, int currentPrice, Sprite typeSprite, int expirationDate)
    {
        _uiInventoryAdditionalDatas.gameObject.SetActive(true);
        SetChargeInfoEnable(true);
        SetChargeInfoActive(true);
        _priceText.text = $"{currentPrice}";
        _upgradeButtonText.text = Strings.Inventory.SELL_BUTTON_TEXT;
        _uiInventoryTypeDatas.SetIngredientTypeData(typeSprite, quantity);
        _uiInventoryAdditionalDatas.SetIngredientAddtionalDatas(expirationDate);
    }

    public void SetRecipeDetailContent(int level, int unlockStar, int upgradePrice, Sprite typeSprite, int originalPrice, int nextPrice)
    {
        if (level == Numbers.RECIPE_MAX_LEVEL) SetChargeInfoActive(false);
        else SetChargeInfo(level, unlockStar, upgradePrice, Strings.Inventory.UPGRADE_SKILL_BUTTON_TEXT);
        _uiInventoryTypeDatas.SetRecipeTypeData(level, typeSprite);
        _uiInventoryAdditionalDatas.SetRecipeAddtionalDatas(originalPrice, nextPrice);
    }

    public void SetKitchenUtensilDetailContent(int level, int requiredStar, int upgradePrice
        , int curSpeed, int curQuantity, int nextSpeed,  int nextQuantity)
    {
        if (level == Numbers.KITCHEN_UTENSIL_MAX_LEVEL) SetChargeInfoActive(false);
        else SetChargeInfo(level, requiredStar, upgradePrice, Strings.Inventory.UPGRADE_BUTTON_TEXT);
        _uiInventoryTypeDatas.SetKitchenUtensilTypeData(level);
        _uiInventoryAdditionalDatas.SetKitchenUtensilAdditionalData(curSpeed, curQuantity, nextSpeed, nextQuantity);
    }

    private void SetChargeInfo(int level, int requiredStar, int requiredPrice, string buttonText)
    {
        SetChargeInfoActive(true);

        _priceText.text = requiredPrice.ToString("N0");
        if (_star < requiredStar)
        {
            SetChargeInfoEnable(false);
            _warningText.text = $"평점 {(int)(requiredStar * 0.1f)} 달성 필요";
        }
        else if (_money < requiredPrice)
        {
            SetChargeInfoEnable(false);
            _warningText.text = "잔액 부족";
        }
        else
        {
            SetChargeInfoEnable(true);
        }

        if (level == Numbers.RECIPE_LOCKED_LEVEL) _upgradeButtonText.text = Strings.Inventory.UNLOCK_BUTTON_TEXT;
        else  _upgradeButtonText.text = buttonText;
    }

    private void SetChargeInfoEnable(bool isEnable)
    {
        if (isEnable)
        {
            _priceText.color = Color.black;
            _upgradeButton.enabled = true;
            _warningText.gameObject.SetActive(false);
        } else
        {
            _priceText.color = Colors.Coral;
            _upgradeButton.enabled = false;
            _warningText.gameObject.SetActive(true);
        }
    }

    private void SetChargeInfoActive(bool isActive)
    {
        _priceTextBackground.SetActive(isActive);
        _upgradeButton.gameObject.SetActive(isActive);
    }
}
