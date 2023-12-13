

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [field:SerializeField]public List<IngredientInfoData> IngredientDatas { get; private set; }
    [field:SerializeField]public List<RecipeInfoData> RecipeInfoDatas{ get; private set; }
    [field:SerializeField]public List<KitchenUtensilInfoData> KitchenUtensilInfoDatas{ get; private set; }
    [field:SerializeField]public List<StoreDecorationInfoSO> StoreDecorationDatas{ get; private set; }
    
    public Inventory()
    {
        IngredientDatas = new List<IngredientInfoData>(20);
        RecipeInfoDatas = new List<RecipeInfoData>(20);
        KitchenUtensilInfoDatas = new List<KitchenUtensilInfoData>(20);
        StoreDecorationDatas = new List<StoreDecorationInfoSO>(20);
    }

    public Inventory(ref PlayerSaveData saveData)
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager가 제대로 초기화 되지 않았습니다");
        
        IngredientDatas = saveData.IngredientDatas;
        RecipeInfoDatas = saveData.RecipeInfoDatas;
        KitchenUtensilInfoDatas = saveData.KitchenUtensilInfoDatas;
        StoreDecorationDatas = new List<StoreDecorationInfoSO>();

        SOCheckUtil.RecheckSO(IngredientDatas);
        SOCheckUtil.RecheckSO(RecipeInfoDatas);
        SOCheckUtil.RecheckSO(KitchenUtensilInfoDatas);
        SOCheckUtil.StringToSO<StoreDecorationInfoSO>(saveData.StoreDecorationDatas, StoreDecorationDatas);

        SOCheckUtil.CheckNewDefaultData<RecipeInfoSO,RecipeInfoData>(dataManager,RecipeInfoDatas);
        SOCheckUtil.CheckNewDefaultData<KitchenUtensilInfoSO,KitchenUtensilInfoData>(dataManager,KitchenUtensilInfoDatas);
        SOCheckUtil.CheckNewDefaultData<StoreDecorationInfoSO>(dataManager,StoreDecorationDatas);
        
        RecipeInfoDatas.Sort((a,b) => a.DefaultData.ID - b.DefaultData.ID);
        KitchenUtensilInfoDatas.Sort((a,b) => a.DefaultData.ID - b.DefaultData.ID);
    }

   
#if UNITY_EDITOR
    public Inventory(Inventory inventory) : this()
    {
        for (int i = 0; i < inventory.IngredientDatas.Count; ++i)
        {
            IngredientDatas.Add(inventory.IngredientDatas[i]);
        }
        for (int i = 0; i < inventory.RecipeInfoDatas.Count; ++i)
        {
            RecipeInfoDatas.Add(inventory.RecipeInfoDatas[i]);
        }
        for (int i = 0; i < inventory.KitchenUtensilInfoDatas.Count; ++i)
        {
            KitchenUtensilInfoDatas.Add(inventory.KitchenUtensilInfoDatas[i]);
        }
        for (int i = 0; i < inventory.StoreDecorationDatas.Count; ++i)
        {
            StoreDecorationDatas.Add(inventory.StoreDecorationDatas[i]);
        }
    }
#endif
    public void AddIngredient(IngredientInfoData data, int currentPrice)
    {
        if (currentPrice != 0) data.PriceAtBuy = currentPrice;
        for (int i = 0; i < IngredientDatas.Count; i++)
        {
            if (IngredientDatas[i].DefaultData.ID == data.DefaultData.ID
                && IngredientDatas[i].ExpirationDate == data.ExpirationDate)
            {
                IngredientDatas[i].Quantity += data.Quantity;
                return;
            } 
        }
        IngredientDatas.Add(data);
    }

    public void SellIngredient(int idx, int quantity)
    {
        IngredientInfoData data = IngredientDatas[idx];
        data.Quantity -= quantity;
        if (data.Quantity <= 0)
        {
            IngredientDatas.RemoveAt(idx);
        }
    }

    public void UpgradeRecipe(int id)
    {
        if (id >= 0 && id < 200)
        {
            for (int i = 0; i < RecipeInfoDatas.Count; i++)
            {
                if (RecipeInfoDatas[i].DefaultData.ID == id)
                {
                    RecipeInfoDatas[i].Level++;
                    AnalyticsManager.AchievementRecipe(RecipeInfoDatas[i].DefaultData.name,RecipeInfoDatas[i].Level);
                    break;
                }
            }
        }
    }

    public void UpgradeKitchenUtensil(int id)
    {
        if (id >= 0 && id < 200)
        {
            for (int i = 0; i < KitchenUtensilInfoDatas.Count; i++)
            {
                if (KitchenUtensilInfoDatas[i].DefaultData.ID == id)
                {
                    KitchenUtensilInfoDatas[i].Level++;
                    break;
                }
            }
        }
    }

    public void BuyStoreDecoration(StoreDecorationInfoSO data)
    {
        StoreDecorationDatas.Add(data);
    }
}
