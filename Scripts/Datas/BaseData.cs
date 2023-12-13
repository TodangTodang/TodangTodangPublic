
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public abstract class BaseData
{
    public ScriptableObject _defaultData;
    public string name;

    public BaseData()
    {
        
    }
    public BaseData(string name)
    {
        this.name = name;
    }
    public void RecheckSO()
    {
        _defaultData = RecheckingSO();
    }

    protected abstract ScriptableObject RecheckingSO();
}
[Serializable]
public class IngredientInfoData : BaseData
{
    public string dataString;
    public IngredientInfoSO DefaultData { get; private set; }
    public int PriceAtBuy;
    public int ExpirationDate;
    public int Quantity;

    public IngredientInfoData(IngredientInfoSO defaultData, int quantity) : base(defaultData.name)
    {
        _defaultData = defaultData;
        DefaultData = defaultData;
        PriceAtBuy = defaultData.BasePrice;
        ExpirationDate = defaultData.BaseExpirationDate;
        Quantity = quantity;
    }

    protected override ScriptableObject RecheckingSO()
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager가 제대로 초기화 되지 않았습니다");
        DefaultData = dataManager.GetDefaultData<IngredientInfoSO>(name);
        return DefaultData;
    }
}
[Serializable]
public class KitchenUtensilInfoData : BaseData
{
    public KitchenUtensilInfoSO DefaultData { get; private set; }
    public int Level;

    public KitchenUtensilInfoData(KitchenUtensilInfoSO defaultData) : base(defaultData.name)
    {
        _defaultData = defaultData;
        DefaultData = defaultData;
        Level = 1;
    }

    protected override ScriptableObject RecheckingSO()
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager가 제대로 초기화 되지 않았습니다");
        DefaultData = dataManager.GetDefaultData<KitchenUtensilInfoSO>(name);
        return DefaultData;throw new NotImplementedException();
    }
}
[Serializable]
public class RecipeInfoData : BaseData
{
    public RecipeInfoSO DefaultData { get; private set; }
    public int Level;

    public RecipeInfoData(RecipeInfoSO defaultData) :base (defaultData.name)
    {
        _defaultData = defaultData;
        DefaultData = defaultData;
        Level = 0;
    }

    protected override ScriptableObject RecheckingSO()
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager가 제대로 초기화 되지 않았습니다");
        DefaultData = dataManager.GetDefaultData<RecipeInfoSO>(name);
        return DefaultData;
    }
}

[Serializable]
public class StoreDecorationInfoData : BaseData
{
    public StoreDecorationInfoSO DefaultData { get; private set; }
    public bool IsSoldOut;

    public StoreDecorationInfoData(StoreDecorationInfoSO defaultData) : base(defaultData.name)
    {
        _defaultData = defaultData;
        DefaultData = defaultData;
        IsSoldOut = false;
    }

    protected override ScriptableObject RecheckingSO()
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager가 제대로 초기화 되지 않았습니다");
        DefaultData = dataManager.GetDefaultData<StoreDecorationInfoSO>(name);
        return DefaultData;
    }
}
