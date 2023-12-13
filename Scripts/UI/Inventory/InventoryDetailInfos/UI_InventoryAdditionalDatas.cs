using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryAdditionalDatas : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _originalInfoText;
    [SerializeField] private TMP_Text _upgradeInfoText;
    [SerializeField] private TMP_Text _expirationDateText;
    [SerializeField] private Image _upgradeIconImage;
    [SerializeField] private GameObject _upgradeInfoBackground;

    public void SetIngredientAddtionalDatas(int expirationDate)
    {
        _titleText.text = Strings.Inventory.EXPIRATION_DATE_TITLE;
        _expirationDateText.gameObject.SetActive(true);
        _expirationDateText.text = $"{expirationDate}일";
        _upgradeInfoBackground.SetActive(false);
    }

    public void SetRecipeAddtionalDatas(int originalPrice, int nextPrice)
    {
        gameObject.SetActive(true);
        _expirationDateText.gameObject.SetActive(false);
        _upgradeInfoBackground.SetActive(true);
        _titleText.text = Strings.Inventory.SKILL_UPGRADE_TITLE;
        SetUpgradeInfos(
                title: Strings.Inventory.SKILL_UPGRADE_TITLE
                , original: originalPrice
                , next: nextPrice
                , isRecipe: true
            );
    }

    public void SetKitchenUtensilAdditionalData(int curSpeed,  int curQuantity, int nextSpeed, int nextQuantity)
    {
        gameObject.SetActive(true);
        _expirationDateText.gameObject.SetActive(false);
        _upgradeInfoBackground.SetActive(true);
        if (curSpeed == nextSpeed)
        {
            SetUpgradeInfos(Strings.Inventory.QUANTITY_UPGRADE_TITLE, curQuantity, nextQuantity);
        }
        else
        {
            SetUpgradeInfos(Strings.Inventory.SPEED_UPGRADE_TITLE, curSpeed, nextSpeed);
        }
    }

    public void SetAdditionalDatas(KitchenUtensilInfoData data)
    {
        KitchenUtensilInfoSO dataSO = data.DefaultData;

        _expirationDateText.gameObject.SetActive(false);

        int level = data.Level;

        gameObject.SetActive(true);
        _upgradeInfoBackground.SetActive(true);

        int idx = level - 1;
        int curSpeed = (level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) ? -1 : dataSO.SpeedUpgradeInfo[idx];
        int curQuantity = (level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) ? -1 : dataSO.QuantityUpgradeInfo[idx];

        int nextSpeed = (level == Numbers.KITCHEN_UTENSIL_MAX_LEVEL) ? -1 : dataSO.SpeedUpgradeInfo[idx + 1];
        int nextQuantity = (level == Numbers.KITCHEN_UTENSIL_MAX_LEVEL) ? -1 : dataSO.QuantityUpgradeInfo[idx + 1];

        if (curSpeed == nextSpeed)
        {
            SetUpgradeInfos(Strings.Inventory.QUANTITY_UPGRADE_TITLE, curQuantity, nextQuantity);
        }
        else
        {
            SetUpgradeInfos(Strings.Inventory.SPEED_UPGRADE_TITLE, curSpeed, nextSpeed);
        }
    }

    // next가 -1이면 현재 Level이 Max Level
    private void SetUpgradeInfos(string title, int original, int next, bool isRecipe = false)
    {
        _titleText.text = title;
        _originalInfoText.text = (original == -1) ? Strings.Inventory.LOCKED_LEVEL_PRICE : $"{original}";
        _upgradeInfoText.text = (next == -1) ? Strings.Inventory.MAX_LEVEL_PRICE : $"{next}";
        _upgradeIconImage.sprite 
            = ResourceManager.Instance.LoadSprite(
                (isRecipe)
                ? Strings.Sprites.INVENTORY_MONEY_ICON 
                : Strings.Sprites.INVENTORY_TIME_ICON
                );
    }
}
