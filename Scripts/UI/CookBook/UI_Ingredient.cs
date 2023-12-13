using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingredient : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;

    public void Init(IngredientInfoSO ingredient)
    {
        _icon.sprite = ingredient.IconSprite;
        _name.text = ingredient.Name.ToString();
    }
}
