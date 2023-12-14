using System.Collections.Generic;
using UnityEngine;

public class InventoryIngredientTabController : InventoryController
{
    private List<IngredientInfoData> _ingredientData;
    private MarketData _marketData;

    public override void InitDatas()
    {
        base.InitDatas();

        if (_marketData == null) _marketData = _gameManager.GetMarketData();
        _ingredientData = _playerData.GetInventory<IngredientInfoData>();

#if UNITY_EDITOR
        Debug.Assert(_marketData != null, "Null Exception : MarketData");
        Debug.Assert(_ingredientData != null, "Null Exception : List<IngredientInfoData>");
#endif
    }

    public override void RefreshTab()
    {
        base.RefreshTab();

        _uiInventory.RefreshScrollView(_ingredientData.Count);

        _slotList = _uiInventory.GetSlotList();
        for (int i = 0; i < _slotList.Length; i++)
        {
            _slotList[i].ItemIdx = i;
            RefreshSlot(_slotList[i]);
        }
    }

    private void RefreshSlot(UI_InventorySlot inventorySlot)
    {
        IngredientInfoData data = _ingredientData[inventorySlot.ItemIdx];
        Sprite icon = data.DefaultData.IconSprite;
        inventorySlot.InitSlotThumbnail(
                itemSprite: icon
                , quantity: data.Quantity
                , expirationDate: data.ExpirationDate
            );
    }

    public override void RefreshDetail(int idx)
    {
        if (idx < 0 || idx >= _ingredientData.Count)
        {
#if UNITY_EDITOR
            Debug.LogError($"Invalid Index : {idx}");
#endif
            return;
        }

        base.RefreshDetail(idx);

        IngredientInfoSO ingredientSO = _ingredientData[idx].DefaultData;

        Sprite icon = ingredientSO.IconSprite;
        string name = ingredientSO.Name;
        string description = ingredientSO.Description;
        int quantity = _ingredientData[idx].Quantity;
        string key = _ingredientData[idx].DefaultData.name;
        int currentPrice = _marketData.GetCurrentIngredientPrice(key);
        Sprite typeSprite = _resourceManager.LoadSprite(
                (_ingredientData[idx].DefaultData.Type == Enums.FoodType.Ricecake)
                ? Strings.Sprites.INVENTORY_RICECAKE_TYPE_ICON
                : Strings.Sprites.INVENTORY_TEA_TYPE_ICON
            );
        int expirationDate = _ingredientData[idx].ExpirationDate;

        _uiInventoryDetail.SetCommonContent(sprite: icon, name: name, description: description);
        _uiInventoryDetail.SetIngredientDetailContent(
                quantity: quantity
                , currentPrice: currentPrice
                , typeSprite: typeSprite
                , expirationDate: expirationDate
            );

        _uiInventoryDetail.SetButtonClickListener(() =>
                {
                    _uiManager.ShowPopup<UI_SliderPopup>(
                        new SliderPopupParameter(
                                sliderMaxValue: quantity
                                , valueConfirmAction: SellIngredient
                            )
                        );
                }
            );
    }

    private void SellIngredient(int quantity)
    {
        bool isAllRefresh = _ingredientData[_curSelectedItemIdx].Quantity == quantity;
        string ingredientName = _ingredientData[_curSelectedItemIdx].DefaultData.name;

        _playerData.SellIngredient(_curSelectedItemIdx, quantity, _marketData.GetCurrentIngredientPrice(ingredientName));
        if (isAllRefresh) RefreshTab();
        else
        {
            RefreshSlot(_slotList[_curSelectedItemIdx]);
            RefreshDetail(_curSelectedItemIdx);
        }

        _uiInventory.UpdatePlayerMoneyUI(_playerData.Money);
        _dataManager.SaveAllData();
        _soundManager.Play(Strings.Sounds.UI_BUYANDSELL);
    }
}
