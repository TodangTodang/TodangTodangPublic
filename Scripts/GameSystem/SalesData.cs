using System.Collections.Generic;

public struct SalesData
{
    public int EarnMoney;
    public int RestMoney; 
    public int TotalCustomerCount;
    public int EnterCustomerCount;
    public int PerfectCustomerCount;
    public int GreatCustomerCount;
    public int SoSoCustomerCount;
    public int AngryCustomerCount;
    public int TotalDisposeCount; 

    public Dictionary<IngredientInfoSO, int> SelledFoods;
    public Dictionary<IngredientInfoSO, int> ExpiredFoods;
}
