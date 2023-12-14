using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerData : Savable
{
    private int _star;
    private int _money;
    private int _date;
    public int Star {
        get
        {
            return _star;
        }
        private set
        {
            if (0 > value)
                _star = 0;
            else if (value >50)
                _star = 50;
            else
                _star = value;
        } 
    }

    public int Money
    {
        get
        {
            return _money;
        }
        private set
        {
          
            if (0 > value)
                _money = 0;
            else if (value >1000000)
                _money = 1000000;
            else
                _money = value;
        }
    }

    public int Date
    {
        get
        {
            return _date;
        }
        private set
        {
            if (1 > value)
                _date = 1;
            else
                _date = value;
        }
    }
    
    public Enums.PlayerDayCycleState DayCycleState { get; private set; }
    
    public Enums.PlayerEndingState EndingState { get; private set; }

    public bool isNeedHelp { get; private set; }
    
    public bool IsCompleteUpgradeTutorial { get; private set; }

    [FormerlySerializedAs("IsNotFirst")] public bool IsNotFirstPlay;

    private Inventory inventory;
    
    private List<ResultInfoSO> unlockedRecipes;

    public event Action PlayerDataChange;

    public PlayerData()
    {
        inventory = new Inventory();
        unlockedRecipes = new List<ResultInfoSO>();
    }

    public override void Init(string json, Param saveParam)
    {
        PlayerSaveData playerSaveData;
        playerSaveData = JsonUtility.FromJson<PlayerSaveData>(json);
        Star = playerSaveData.Star;
        Money = playerSaveData.Money;
        Date = playerSaveData.Date;
        IsNotFirstPlay = playerSaveData.isFirstPlay;
        DayCycleState = playerSaveData.DayCycleState;
        EndingState = playerSaveData.EndingState;
        isNeedHelp = playerSaveData.isNeedHelp;
        IsCompleteUpgradeTutorial = playerSaveData.IsCompleteUpgradeTutorial;
        inventory = new Inventory(ref playerSaveData);

        List<string> NewUnlockedList = playerSaveData.NewUnLockedRecipeList;
        unlockedRecipes = new List<ResultInfoSO>();
        SOCheckUtil.StringToSO<ResultInfoSO>(NewUnlockedList,unlockedRecipes);
        
    }

    public override string GetJsonData()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData();
        
        playerSaveData.Money = Money;
        playerSaveData.Star = Star;
        playerSaveData.Date = Date;
        playerSaveData.isFirstPlay = IsNotFirstPlay;
        playerSaveData.DayCycleState = DayCycleState;
        playerSaveData.EndingState = EndingState;
        playerSaveData.isNeedHelp = isNeedHelp;
        playerSaveData.isFirstPlay = IsNotFirstPlay;
        playerSaveData.IsCompleteUpgradeTutorial = IsCompleteUpgradeTutorial;
        playerSaveData.IngredientDatas = GetInventory<IngredientInfoData>();
        playerSaveData.RecipeInfoDatas = GetInventory<RecipeInfoData>();
        playerSaveData.KitchenUtensilInfoDatas = GetInventory<KitchenUtensilInfoData>();
        
        List<StoreDecorationInfoSO> decoList = GetInventory<StoreDecorationInfoSO>();
        playerSaveData.StoreDecorationDatas = new List<string>(decoList.Count);
        for (int i = 0; i < decoList.Count; i++)
        {
            playerSaveData.StoreDecorationDatas.Add(decoList[i].name);            
        }
        
        List<string> NewUnlockedRecipeStrList = new List<string>(unlockedRecipes.Count);
        
        for (int i = 0; i < unlockedRecipes.Count; i++)
        {
            NewUnlockedRecipeStrList.Add(unlockedRecipes[i].name);
        }

        playerSaveData.NewUnLockedRecipeList = NewUnlockedRecipeStrList;
        
        string jsonData = JsonUtility.ToJson(playerSaveData);
        
        return jsonData;
    }
    
    public void EndDay()
    {
        Date += 1;
        UpdatePlayerData();
    }

    public void UpdateStar(int AddStar)
    {
        Star += AddStar;
        Star = Math.Max(0, Star);
        UpdatePlayerData();
    }

    public void UpdateMoney(int spendedMoney)
    {
        Money += spendedMoney;
        Money = Math.Max(0, Money);
        UpdatePlayerData();
    }

    public void UpdateDayCycleState(Enums.PlayerDayCycleState state)
    {
        DayCycleState = state;
    }

    public void UpdateEndingState(Enums.PlayerEndingState state)
    {
        EndingState = state;
    }

    public void UpdateNeedHelp(bool value)
    {
        isNeedHelp = value;
    }

    public void UpdateCompleteUpgradeTutorial(bool complete)
    {
        IsCompleteUpgradeTutorial = complete;
    }

    public List<IngredientInfoData> RemoveExpiredItem()
    {
        List<IngredientInfoData> expiredList = null;
        List<IngredientInfoData> currentItem = inventory.IngredientDatas;
        for (int i = 0; i < currentItem.Count; ++i)
        {
            currentItem[i].ExpirationDate -= 1;
            if (currentItem[i].ExpirationDate == 0)
            {
                if (expiredList == null)
                    expiredList = new List<IngredientInfoData>();
                expiredList.Add(currentItem[i]);
            }

        }
        if (expiredList != null)
        {
            foreach (var item in expiredList)
            {
                currentItem.Remove(item);
            }
        }

        return expiredList;
    }

    public List<T> GetInventory<T>()
    {
        Type type = typeof(T);
        if (type == typeof(IngredientInfoData))
        {
            return inventory.IngredientDatas as List<T>;
        }
        else if (type == typeof(RecipeInfoData))
        {
            return inventory.RecipeInfoDatas as List<T>;
        }
        else if (type == typeof(KitchenUtensilInfoData))
        {
            return inventory.KitchenUtensilInfoDatas as List<T>;
        }
        else if (type == typeof(StoreDecorationInfoSO))
        {
            return inventory.StoreDecorationDatas as List<T>;
        }

        return null;
    }

    public void UpdatePlayerData()
    {
        if (PlayerDataChange != null)
            PlayerDataChange.Invoke();
    }

    public void AddIngredient(IngredientInfoData data, int currentPrice)
    {
        inventory.AddIngredient(data, currentPrice);
        if (currentPrice != 0) UpdateMoney(-data.Quantity * currentPrice);
    }

    public void SellIngredient(int idx, int quantity, int currentPrice)
    {
        inventory.SellIngredient(idx, quantity);
        UpdateMoney(quantity * currentPrice);
    }

    public void UpgradeRecipe(int idx, int price)
    {
        inventory.UpgradeRecipe(idx);
        if (price != 0) UpdateMoney(-price);

        AddUnlockedRecipe(idx);
    }

    public void UpgradeKitchenUtensil(int id, int price)
    {
        inventory.UpgradeKitchenUtensil(id);
        UpdateMoney(-price);
    }

    public void BuyStoreDecoration(StoreDecorationInfoSO data)
    {
        inventory.BuyStoreDecoration(data);
        UpdateMoney(-data.Price);
    }

    public List<ResultInfoSO> GetUnlockedRecipe()
    {
        if (unlockedRecipes == null) unlockedRecipes = new List<ResultInfoSO>();    //TODO: ������ ���� �� ����
        return unlockedRecipes;
    }

    private void AddUnlockedRecipe(int id)
    {
        if (unlockedRecipes == null) unlockedRecipes = new List<ResultInfoSO>();    //TODO: ������ ���� �� ����


        if (id < 100)
        {
            for (int i = 0; i < inventory.RecipeInfoDatas.Count; i++)
            {
                if (inventory.RecipeInfoDatas[i].DefaultData.ID == id)
                {
                    if (inventory.RecipeInfoDatas[i].Level == 1)
                        unlockedRecipes.Add(inventory.RecipeInfoDatas[i].DefaultData.ResultSO[0] as ResultInfoSO);
                }
            }
        }
        else
        {
            List<RecipeInfoData> teaDatas = new List<RecipeInfoData>();
            for (int i = 0; i < inventory.RecipeInfoDatas.Count; i++)
            {
                if (inventory.RecipeInfoDatas[i].DefaultData.Type == Enums.FoodType.Tea && inventory.RecipeInfoDatas[i].Level > 0)
                {
                    teaDatas.Add(inventory.RecipeInfoDatas[i]);
                }
            }

            if (teaDatas.Count == 1)
            {
                IngredientInfoSO greenTea = inventory.RecipeInfoDatas.Find(tea => tea.DefaultData.ID == 100).DefaultData.ResultSO[0];
                unlockedRecipes.Add(greenTea as ResultInfoSO);
            }
        }
    }

    public void RemoveUnlockedRecipe(ResultInfoSO ingredient)
    {
        if (unlockedRecipes == null) unlockedRecipes = new List<ResultInfoSO>();    //TODO: ������ ���� �� ����

        unlockedRecipes.Remove(ingredient);
    }

    public void OnBankruptcy()
    {
        Money += 5000;
        UpdateEndingState(Enums.PlayerEndingState.BankruptcyEnding);
    }

#if UNITY_EDITOR
    public PlayerData(PlayerData clone)
    {
        Star = clone.Star;
        Money = clone.Money;
        Date = clone.Date;

        inventory = new Inventory(clone.inventory);
        DataManager dataManager = DataManager.Instance;
        
        DebugUtil.AssertNullException(dataManager,nameof(dataManager));
        dataManager.CancelRegistSaveData(this);
    }
    
    public Inventory GetInventory()
    {
        return inventory;
    }
    
    public void SetDay(int day)
    {
        _date = Math.Min(day, 30);
        UpdatePlayerData();
    }

    public void SetStar(int star)
    {
        _star = Math.Min(star,55);
        UpdatePlayerData();
    }

    public void SetMoney(int money)
    {
        _money = Math.Min(money, 1000000);
        UpdatePlayerData();
    }
#endif
    
}
