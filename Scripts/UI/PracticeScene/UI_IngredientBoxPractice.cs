using System.Collections.Generic;

public class UI_IngredientBoxPractice : UI_IngredientBox
{
    private List<RecipeInfoData> _recipeInfoData;

    public override void SelectItem()
    {
        if (currentSlot == null) return;
        OnSelection?.Invoke(currentSlot.ingredient);

        CloseUI();
    }

    protected override void ResetData()
    {
        _gameManager = GameManager.Instance;
        _playerData = _gameManager.GetPlayerData();
        _recipeInfoData = _playerData.GetInventory<RecipeInfoData>();
        _ricecakeIngredients = new List<IngredientInfoData>();
        _teaIngredients = new List<IngredientInfoData>();

        HashSet<IngredientInfoSO> ingredientSO = new HashSet<IngredientInfoSO>();

        foreach (RecipeInfoData item in _recipeInfoData)
        {
            if (item.Level < 0) continue;

            IngredientInfoSO[] ingredients = item.DefaultData.IngredientInfoSO;
            foreach (IngredientInfoSO ingredient in ingredients)
            {
                ingredientSO.Add(ingredient);
            }
        }

        foreach (IngredientInfoSO ingredient in ingredientSO)
        {
            int quantity = 1;
            IngredientInfoData newData = new IngredientInfoData(ingredient, quantity);

            if (newData.DefaultData.Type == Enums.FoodType.Ricecake)
            {
                _ricecakeIngredients.Add(newData);
            }
            else if (newData.DefaultData.Type == Enums.FoodType.Tea)
            {
                _teaIngredients.Add(newData);
            }
        }        
    }
}
