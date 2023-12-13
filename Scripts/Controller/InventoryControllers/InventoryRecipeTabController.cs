using System.Collections.Generic;
using UnityEngine;

public class InventoryRecipeTabController : InventoryController
{
    private List<RecipeInfoData> _recipeData;

    public override void InitDatas()
    {
        base.InitDatas();
        _recipeData = _playerData.GetInventory<RecipeInfoData>();
#if UNITY_EDITOR
        Debug.Assert(_recipeData != null, "Null Exception : List<RecipeInfoData>");
#endif

        // Day06 Tea Event Exception
        if (_playerData.Date < Numbers.TEA_EVENT_DAY)
        {
            List<RecipeInfoData> tmpRecipeList = new List<RecipeInfoData>();
            for (int i = 0; i < _recipeData.Count; i++)
            {
                if (_recipeData[i].DefaultData.Type == Enums.FoodType.Ricecake) tmpRecipeList.Add(_recipeData[i]);
            }
            _recipeData = tmpRecipeList;
        }

        _recipeData.Sort((a, b) =>
        {
            if (a.DefaultData.Type != b.DefaultData.Type)
            {
                if (a.DefaultData.Type == Enums.FoodType.Ricecake && b.DefaultData.Type == Enums.FoodType.Tea) return -1;
                else return 1;
            }
            else
            {
                if (a.DefaultData.ID < b.DefaultData.ID) return -1;
                else if (a.DefaultData.ID == b.DefaultData.ID) return 0;
                else return 1;
            }
        });
    }

    public override void RefreshTab()
    {
        base.RefreshTab();

        _uiInventory.RefreshScrollView(_recipeData.Count);

        _slotList = _uiInventory.GetSlotList();
        for (int i = 0; i < _slotList.Length; i++)
        {
            _slotList[i].ItemIdx = i;
            RefreshSlot(_slotList[i]);
        }
    }

    private void RefreshSlot(UI_InventorySlot inventorySlot)
    {    
        RecipeInfoData data = _recipeData[inventorySlot.ItemIdx];
        Sprite icon = data.DefaultData.IconSprite;
        inventorySlot.InitSlotThumbnail(
                itemSprite: icon
                , isLocked: (data.Level == 0)
            );
    }

    public override void RefreshDetail(int idx)
    {
        if (idx < 0 || idx >= _recipeData.Count)
        {
#if UNITY_EDITOR
            Debug.LogError($"Invalid Index : {idx}");
#endif
            return;
        }
        base.RefreshDetail(idx);

        RecipeInfoSO recipeSO = _recipeData[idx].DefaultData;
        Sprite icon = recipeSO.IconSprite;
        string name = recipeSO.Name;
        string description = recipeSO.Description;
        _uiInventoryDetail.SetPlayerInfo(_playerData.Star, _playerData.Money);
        _uiInventoryDetail.SetCommonContent(sprite: icon, name: name, description: description);

        int level = _recipeData[idx].Level;
        int unlockStar = -1;
        int upgradePrice = -1;
        if (level != Numbers.RECIPE_MAX_LEVEL)
        {
            unlockStar = recipeSO.UnlockStarRating;
            upgradePrice = recipeSO.UpgradePrice[level];
        }
        
        Sprite typeSprite = _resourceManager.LoadSprite(
                (recipeSO.Type == Enums.FoodType.Ricecake)
                ? Strings.Sprites.INVENTORY_RICECAKE_TYPE_ICON
                : Strings.Sprites.INVENTORY_TEA_TYPE_ICON
            );

        int valueIdx = level - 1;
        _uiInventoryDetail.SetRecipeDetailContent(
                level: level
                , unlockStar: unlockStar
                , upgradePrice: upgradePrice
                , typeSprite: typeSprite
                , originalPrice: (level == Numbers.RECIPE_LOCKED_LEVEL) ? -1 : recipeSO.Price[valueIdx]
                , nextPrice: (level == Numbers.RECIPE_MAX_LEVEL) ? -1 : recipeSO.Price[valueIdx + 1]
            );

        _uiInventoryDetail.SetButtonClickListener(UpgradeRecipe);
    }

    private void UpgradeRecipe()
    {
        int level = _recipeData[_curSelectedItemIdx].Level;
        int price = _recipeData[_curSelectedItemIdx].DefaultData.UpgradePrice[level];

        #region Unlock Recipe
        if (level == 0)
        {
            IngredientInfoSO[] newIngredients = _recipeData[_curSelectedItemIdx].DefaultData.IngredientInfoSO;
            Sprite[] iconList = new Sprite[newIngredients.Length];

            for (int i = 0; i < newIngredients.Length; i++)
            {
                int ingredientNum = 5;
                if (newIngredients[i].Type == Enums.FoodType.Tea) ingredientNum = 10;
                IngredientInfoData ingredient = new IngredientInfoData(newIngredients[i], ingredientNum);
                _playerData.AddIngredient(ingredient, 0);

                iconList[i] = newIngredients[i].IconSprite;
            }

            UIManager.Instance.ShowPopup<UI_ImagePopup>(
                new ImagePopupParameter(
                    content: $"{Strings.Inventory.UNLOCK_RECIPE1}\n{_recipeData[_curSelectedItemIdx].DefaultData.Name}{Strings.Inventory.UNLOCK_RECIPE2}"
                    , spriteList: iconList
                    )
                );
        }
        #endregion

        _playerData.UpgradeRecipe(_recipeData[_curSelectedItemIdx].DefaultData.ID, price);
        RefreshSlot(_slotList[_curSelectedItemIdx]);
        RefreshDetail(_curSelectedItemIdx);
        _uiInventory.UpdatePlayerMoneyUI(_playerData.Money);
        if (_playerData.IsCompleteUpgradeTutorial) _dataManager.SaveAllData();
        _soundManager.Play(Strings.Sounds.UI_BUYANDSELL);
    }
}
