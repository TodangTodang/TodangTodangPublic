using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/RecipeInfoSO")]
public class RecipeInfoSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private Enums.FoodType type;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private int unlockStarRating;
    [SerializeField] private int[] price;
    [SerializeField] private int[] upgradePrice;
    [SerializeField] private IngredientInfoSO[] ingredientInfoSO;
    [SerializeField] private IngredientInfoSO[] resultSO;
    public int ID => id;
    public string Name => name;
    public string Description => description;
    public Enums.FoodType Type => type;
    public Sprite IconSprite => iconSprite;
    public int UnlockStarRating => unlockStarRating;
    public int[] Price => price;
    public int[] UpgradePrice => upgradePrice;
    public IngredientInfoSO[] IngredientInfoSO => ingredientInfoSO;

    public IngredientInfoSO[] ResultSO => resultSO;
}
