using System.Collections.Generic;
using UnityEngine;

public class InventoryKitchenTabController : InventoryController
{
    private List<KitchenUtensilInfoData> _kitchenUtensilData;

    public override void InitDatas()
    {
        base.InitDatas();
        _kitchenUtensilData = _playerData.GetInventory<KitchenUtensilInfoData>();
#if UNITY_EDITOR
        Debug.Assert(_kitchenUtensilData != null, "Null Exception : List<KitchenUtensilInfoData>");
#endif

        // Day06 Tea Event Exception
        if (_playerData.Date < Numbers.TEA_EVENT_DAY)
        {
            List<KitchenUtensilInfoData> tmpKitchenUtensilList = new List<KitchenUtensilInfoData>();
            for (int i = 0; i < _kitchenUtensilData.Count; i++)
            {
                if (_kitchenUtensilData[i].DefaultData.ID != Numbers.KitchenUtensilID.TEA_POT) 
                    tmpKitchenUtensilList.Add(_kitchenUtensilData[i]);
            }
            _kitchenUtensilData = tmpKitchenUtensilList;
        }

        _kitchenUtensilData.Sort((a, b) =>
            {
                if (a.DefaultData.ID < b.DefaultData.ID) return -1;
                else if (a.DefaultData.ID == b.DefaultData.ID) return 0;
                else return 1;
            });
    }

    public override void RefreshTab()
    {
        base.RefreshTab();

        _uiInventory.RefreshScrollView(_kitchenUtensilData.Count);

        _slotList = _uiInventory.GetSlotList();
        for (int i = 0; i < _slotList.Length; i++)
        {
            _slotList[i].ItemIdx = i;
            RefreshSlot(_slotList[i]);
        }
    }

    private void RefreshSlot(UI_InventorySlot inventorySlot)
    {
        KitchenUtensilInfoData data = _kitchenUtensilData[inventorySlot.ItemIdx];
        Sprite icon = data.DefaultData.IconSprite;
        inventorySlot.InitSlotThumbnail(
                itemSprite: icon
                , isLocked: (data.Level == 0)
            );
    }

    public override void RefreshDetail(int idx)
    {
        if (idx < 0 || idx >= _kitchenUtensilData.Count)
        {
#if UNITY_EDITOR
            Debug.LogError($"Invalid Index : {idx}");
#endif
            return;
        }

        base.RefreshDetail(idx);

        KitchenUtensilInfoSO kitchenUtensilSO = _kitchenUtensilData[idx].DefaultData;
        Sprite icon = kitchenUtensilSO.IconSprite;
        string name = kitchenUtensilSO.Name;
        string description = kitchenUtensilSO.Description;
        _uiInventoryDetail.SetPlayerInfo(_playerData.Star, _playerData.Money);
        _uiInventoryDetail.SetCommonContent(sprite: icon, name: name, description: description);

        int level = _kitchenUtensilData[idx].Level;
        int requiredStar = -1;
        int upgradePrice = -1;
        if (level != Numbers.RECIPE_MAX_LEVEL)
        {
            requiredStar = kitchenUtensilSO.RequiredStarRating[level];
            upgradePrice = kitchenUtensilSO.UpgradePrice[level];
        }

        int valueIdx = level - 1;
        int curSpeed = (level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) ? -1 : kitchenUtensilSO.SpeedUpgradeInfo[valueIdx];
        int curQuantity = (level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) ? -1 : kitchenUtensilSO.QuantityUpgradeInfo[valueIdx];

        int nextSpeed = (level == Numbers.KITCHEN_UTENSIL_MAX_LEVEL) ? -1 : kitchenUtensilSO.SpeedUpgradeInfo[valueIdx + 1];
        int nextQuantity = (level == Numbers.KITCHEN_UTENSIL_MAX_LEVEL) ? -1 : kitchenUtensilSO.QuantityUpgradeInfo[valueIdx + 1];


        _uiInventoryDetail.SetKitchenUtensilDetailContent(
                level: level
                , requiredStar: requiredStar
                , upgradePrice: upgradePrice
                , curSpeed: curSpeed
                , curQuantity: curQuantity
                , nextSpeed: nextSpeed
                , nextQuantity: nextQuantity
            );

        _uiInventoryDetail.SetButtonClickListener(UpgradeKitchenUtensil);
    }

    private void UpgradeKitchenUtensil()
    {
        int level = _kitchenUtensilData[_curSelectedItemIdx].Level;
        int price = _kitchenUtensilData[_curSelectedItemIdx].DefaultData.UpgradePrice[level];
        _playerData.UpgradeKitchenUtensil(_kitchenUtensilData[_curSelectedItemIdx].DefaultData.ID, price);

        RefreshSlot(_slotList[_curSelectedItemIdx]);
        RefreshDetail(_curSelectedItemIdx);
        _uiInventory.UpdatePlayerMoneyUI(_playerData.Money);

        if (_playerData.IsCompleteUpgradeTutorial) _dataManager.SaveAllData();
        _soundManager.Play(Strings.Sounds.UI_BUYANDSELL);
    }
}
