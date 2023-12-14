using UnityEngine;

[CreateAssetMenu(fileName = "IngredientInfoSO", menuName = "Ingredients/IngredientInfoSO")]
public class IngredientInfoSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private Enums.FoodType type;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private int basePrice;
    [SerializeField] private int baseExpirationDate;
    [SerializeField] private GameObject prefab;

    public int ID => id;
    public string Name => name; 
    public string Description => description;
    public Enums.FoodType Type => type;
    public Sprite IconSprite => iconSprite;
    public int BasePrice => basePrice;
    public int BaseExpirationDate => baseExpirationDate;
    public GameObject Prefab => prefab;
}
