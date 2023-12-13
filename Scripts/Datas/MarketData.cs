using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketData : Savable
{
    // 기본 데이터
    private IngredientInfoSO[] _ingredientInfoList;

    // 재료 현재 가격
    private Dictionary<string, int> _ingredientPrices;

    // 판매 가능한 재료 
    private Dictionary<string, bool> _isSellable;
    
    public MarketData() : base()
    { 
        _ingredientPrices = new Dictionary<string, int>();
        _isSellable = new Dictionary<string, bool>();
    }
    
    public MarketData(IngredientInfoSO[] ingredientInfoList) : base()
    {
        _ingredientPrices = new Dictionary<string, int>();
        _isSellable = new Dictionary<string, bool>();
        _ingredientInfoList = ingredientInfoList;
    }

    public IngredientInfoSO[] GetIngredientInfoList()
    {
        return _ingredientInfoList;
    }

    public Dictionary<string, int> GetIngredientPrices()
    {
        return _ingredientPrices;
    }

    public Dictionary<string, bool> GetIsSellableDatas()
    {
        return _isSellable;
    }

    public int GetCurrentIngredientPrice(string name)
    {
        return _ingredientPrices[name];
    }

    public void UpdateIngredientPrice(string name, int price)
    {
        if (_ingredientPrices.ContainsKey(name))
        {
            _ingredientPrices[name] = price;
        }
    }

    public List<IngredientInfoSO> GetCurrentIngredients()
    {
        List<IngredientInfoSO> currentIngredients = new List<IngredientInfoSO>();

        foreach (var ingredient in _ingredientInfoList)
        {
            if (_isSellable[ingredient.name] == true)
            {
                currentIngredients.Add(ingredient);
            }
        }

        return currentIngredients;
    }

    public void UpdateCurrentIngredients(string name)
    {
        _isSellable[name] = true;
    }

    public override void Init(string json, Param saveParam = null)
    {
        MarketDataParam marketDataParam = saveParam as MarketDataParam;
        MarketSaveData data = JsonUtility.FromJson<MarketSaveData>(json);
        _ingredientInfoList = marketDataParam.IngredientInfoSos;

        if (data != null)
        {
            for (int i = 0; i < data.IngredientPriceStrs.Count; ++i)
            {
                _ingredientPrices.Add(data.IngredientPriceStrs[i],data.IngredientPriceValues[i]);
                _isSellable.Add(data.IsSellableStrs[i],data.IsSellableValues[i]);
            }
        }
    }

    public override string GetJsonData()
    {
        MarketSaveData marketSaveData = new MarketSaveData();
        marketSaveData.IngredientPriceStrs = new List<string>();
        marketSaveData.IngredientPriceValues= new List<int>();
        marketSaveData.IsSellableStrs = new List<string>();
        marketSaveData.IsSellableValues = new List<bool>();
        foreach (KeyValuePair<string, int> pricesKeyValue in GetIngredientPrices())
        {
            marketSaveData.IngredientPriceStrs.Add(pricesKeyValue.Key);
            marketSaveData.IngredientPriceValues.Add(pricesKeyValue.Value);
        }
        
        foreach (KeyValuePair<string, bool> sellableKeyValue in GetIsSellableDatas())
        {
            marketSaveData.IsSellableStrs.Add(sellableKeyValue.Key);
            marketSaveData.IsSellableValues.Add(sellableKeyValue.Value);
        }
        
        string jsonData = JsonUtility.ToJson(marketSaveData);
        return jsonData;
    }

    public class MarketDataParam : Param
    {
        public IngredientInfoSO[] IngredientInfoSos;
    }
}
