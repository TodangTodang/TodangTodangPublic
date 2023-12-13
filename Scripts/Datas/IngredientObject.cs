using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private IngredientInfoSO ingredientObjectSO;

    public int CurrentPrice { get; set; }
    public int CurrentExpirationDate { get; set; }
    public int CurrentQuantity { get; set; }

    public IngredientInfoSO GetIngredientObjectSO()
    {
        return ingredientObjectSO;
    }

    private void Start()
    {
        InitializeFromDataSO(); 
    }

    private void InitializeFromDataSO()
    {
        CurrentPrice = ingredientObjectSO.BasePrice;
        CurrentExpirationDate = ingredientObjectSO.BaseExpirationDate;
    }
}
