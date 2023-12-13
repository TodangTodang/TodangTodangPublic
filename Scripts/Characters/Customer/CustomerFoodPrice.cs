
public class CustomerFoodPrice
{
    public IngredientInfoSO Food { get; private set; }
    public int CurrentPrice { get; private set; }

    public CustomerFoodPrice(IngredientInfoSO food, int currentPrice)
    {
#if UNITY_EDITOR
        DebugUtil.AssertNullException(food,nameof(food));
#endif
        Food = food;
        CurrentPrice = currentPrice;
    }
}
