using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryTypeDatas : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private Image _typeImage;

    public void SetIngredientTypeData(Sprite typeSprite, int quantity)
    {
        _levelText.gameObject.SetActive(false);
        _typeImage.gameObject.SetActive(true);
        _typeImage.sprite = typeSprite;
        _quantityText.gameObject.SetActive(true);
        _quantityText.text = $"수량 {quantity}";
    }

    public void SetRecipeTypeData(int level, Sprite typeSprite)
    {
        _quantityText.gameObject.SetActive(false);
        if (level == Numbers.RECIPE_LOCKED_LEVEL) _levelText.gameObject.SetActive(false);
        else
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"Lv {level}";
        }
        _typeImage.gameObject.SetActive(true);
        _typeImage.sprite = typeSprite;
    }

    public void SetKitchenUtensilTypeData(int level)
    {
        _quantityText.gameObject.SetActive(false);
        _typeImage.gameObject.SetActive(false);
        if (level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) _levelText.gameObject.SetActive(false);
        else
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"Lv {level}";
        }
    }


    public void SetTypeDatas(KitchenUtensilInfoData data)
    {
        _quantityText.gameObject.SetActive(false);

        if (data.Level == Numbers.KITCHEN_UTENSIL_LOCKED_LEVEL) _levelText.gameObject.SetActive(false);
        else
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"Lv {data.Level}";
        }

        _typeImage.gameObject.SetActive(false);
    }
}
