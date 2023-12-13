using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private GameManager _gameManager;
    private PlayerData _playerData;

    [SerializeField]private EffectsData _effectsData;
    
    public bool isInit {get; private set;}
    public void Init()
    {
        _gameManager = GameManager.Instance;
        Debug.Assert(_gameManager,"_gameManager이 제대로 초기화가 안됬습니다");
        InitData();
        
        GC.Collect();
    }

    public void EraseAllData()
    {
        _effectsData.EraseAllData();
    }

    public void InitData()
    {
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"dataManager이 제대로 설정이 안됫습니다.");
        
        _playerData = _gameManager.GetPlayerData();
        Debug.Assert(_playerData!=null,"playerData가 초기화가 제대로 안됬습니다");
        
        dataManager.Load<EffectsData>(out _effectsData);
    }

    public float GetCustomerEffectRate(CustomerEffectType type)
    {
        if (!isInit)
            Init();
        float rate = 0;
        for (int i = 0; i < _effectsData.CustomerEffects.Count; ++i)
        {
            if (_effectsData.CustomerEffects[i].EffectType == type)
            {
                rate += _effectsData.CustomerEffects[i].EffectRate;
            }
        }

        return rate;
    }

    public void AddEffectInQueue(EffectSO so, int value)
    {
        _effectsData._effectQueue.Add(new SoAndInt(so, value));
    }

    public void AddEffect(EffectIngredientSO effectIngredientSo, int durationDay)
    {
        _effectsData.AddEffect(effectIngredientSo,durationDay);
    }
    public void AddEffect(EffectCustomerSO effectCustomerSo, int durationDay)
    {
        _effectsData.AddEffect(effectCustomerSo,durationDay);
    }

    public void ApplyEffect(EffectIngredientSO effectIngredientSo, int durationDay)
    {
        AddEffect(effectIngredientSo,durationDay);

        MarketData data = _gameManager.GetMarketData();

        int AddPrice = (int)(effectIngredientSo.Food.BasePrice * effectIngredientSo.EffectRate);
        int currentPrice = data.GetCurrentIngredientPrice(effectIngredientSo.Food.name);
        
        data.UpdateIngredientPrice(effectIngredientSo.Food.name,currentPrice + AddPrice);
    }

    public void UnlockUtensil(EffectCustomerUtensilSO  effectCustomerUtensilSo)
    {
        _playerData.UpgradeKitchenUtensil(effectCustomerUtensilSo.KitchenUtensilInfoSos.ID,0);
    }

    public void AddIngredient(EffectCustomerAddIngredientSO effectCustomerAddIngredientSo, int count)
    {
        PlayerData playerData = _gameManager.GetPlayerData();
        IngredientInfoData data = new IngredientInfoData(effectCustomerAddIngredientSo.ingredientSos, count);
        playerData.AddIngredient(data, 0);
    }

    public void UnlockRecipe(EffectCustomerAddRecipeSO effectCustomerAddRecipeSo)
    {
        _playerData.UpgradeRecipe(effectCustomerAddRecipeSo.RecipeSos.ID,0);
    }

    public void SpendDay()
    {
        ApplyInEffectQueue();
        _effectsData.SpendDay();
        
        Debug.Log($"남아있는 효과{_effectsData.CustomerEffectRemainDays.Count}");
    }

    private void ApplyInEffectQueue()
    {
        while (_effectsData._effectQueue.Count > 0)
        {
            SoAndInt effectSo = _effectsData._effectQueue[_effectsData._effectQueue.Count-1];

            if (effectSo.effectSo is EffectIngredientSO)
            {
                ApplyEffect(effectSo.effectSo as EffectIngredientSO, effectSo.value);
            }else if (effectSo.effectSo  is EffectCustomerSO)
            {
                AddEffect(effectSo.effectSo as EffectCustomerSO, effectSo.value);
            }
            _effectsData._effectQueue.RemoveAt(_effectsData._effectQueue.Count-1);
        }
    }
}
[Serializable]
public class EffectsData : Savable
{
    [field:SerializeField]public List<EffectIngredientSO> IngredientEffects { get; private set; }
    [field:SerializeField]public List<EffectCustomerSO> CustomerEffects { get; private set; }
    [field:SerializeField]public List<int> IngredientEffectRemainDays{ get; private set; }
    [field:SerializeField]public List<int> CustomerEffectRemainDays{ get; private set; }
    [field:SerializeField]public List<SoAndInt> _effectQueue { get; private set; }

    public EffectsData()
    {
        IngredientEffects = new List<EffectIngredientSO>();
        CustomerEffects = new List<EffectCustomerSO>();
        IngredientEffectRemainDays = new List<int>();
        CustomerEffectRemainDays = new List<int>();
        _effectQueue = new List<SoAndInt>();
    }
    public void AddEffect(EffectIngredientSO effectIngredientSo, int durationDay)
    {
        IngredientEffects.Add(effectIngredientSo);
        IngredientEffectRemainDays.Add(durationDay);
    }
    public void AddEffect(EffectCustomerSO effectCustomerSo, int durationDay)
    {
        CustomerEffects.Add(effectCustomerSo);
        CustomerEffectRemainDays.Add(durationDay);
    }
    
    public void EraseAllData()
    {
        IngredientEffects.Clear();
        CustomerEffects.Clear();
        IngredientEffectRemainDays.Clear();
        CustomerEffectRemainDays.Clear();
        _effectQueue.Clear();
    }
    
    public void SpendDay()
    {
        for (int i = 0; i < CustomerEffects.Count; ++i)
        {
            CustomerEffectRemainDays[i] -= 1;
            if (CustomerEffectRemainDays[i] == 0)
            {
                Debug.Log($"{CustomerEffects[i].EffectType} {CustomerEffectRemainDays[i]}");
                CustomerEffectRemainDays.RemoveAt(i);
                CustomerEffects.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < IngredientEffects.Count; ++i)
        {
            IngredientEffectRemainDays[i] -= 1;
            if (IngredientEffectRemainDays[i] == 0)
            {
                MarketData data = GameManager.Instance.GetMarketData();
                int AddPrice = (int)(IngredientEffects[i].Food.BasePrice * IngredientEffects[i].EffectRate);
                int currentPrice = data.GetCurrentIngredientPrice(IngredientEffects[i].Food.name);
        
                data.UpdateIngredientPrice(IngredientEffects[i].Food.name,currentPrice - AddPrice);
                IngredientEffects.RemoveAt(i);
                IngredientEffectRemainDays.RemoveAt(i);
                i--;
            }           
        }
        
        Debug.Log($"남아있는 효과{CustomerEffectRemainDays.Count}");
    }



    public override void Init(string json, Param saveParam = null)
    {
        EffectSaveData effectSaveData = JsonUtility.FromJson<EffectSaveData>(json);
        EffectsDataParam effectsDataParam = saveParam as EffectsDataParam;
        
        SOCheckUtil.StringToSO(effectSaveData.IngredientEffects,IngredientEffects);
        SOCheckUtil.StringToSO(effectSaveData.CustomerEffects,CustomerEffects);
        IngredientEffectRemainDays = effectSaveData.IngredientEffectRemainDays;
        CustomerEffectRemainDays = effectSaveData.CustomerEffectRemainDays;
            
        for (int i = 0; i < effectSaveData.EffectQueueSOString.Count; ++i)
        {
            EffectSO so = effectsDataParam.dataManager.GetDefaultData<EffectSO>(effectSaveData.EffectQueueSOString[i]);
            _effectQueue.Add(new SoAndInt(so,effectSaveData.EffectQueueValue[i]));
        }
    }

    public override string GetJsonData()
    {
        EffectSaveData effectData = new EffectSaveData();
        effectData.IngredientEffects = new List<string>();
        for (int i = 0; i < IngredientEffects.Count; i++)
        {
            effectData.IngredientEffects.Add(IngredientEffects[i].name);
        }

        effectData.CustomerEffects = new List<string>();
        for (int i = 0; i < CustomerEffects.Count; i++)
        {
            effectData.CustomerEffects.Add(CustomerEffects[i].name);
        }

        effectData.IngredientEffectRemainDays = IngredientEffectRemainDays;
        effectData.CustomerEffectRemainDays = CustomerEffectRemainDays;

        effectData.EffectQueueSOString = new List<string>();
        effectData.EffectQueueValue = new List<int>();
        foreach (var queued in _effectQueue)
        {
            effectData.EffectQueueSOString.Add(queued.effectSo.name);
            effectData.EffectQueueValue.Add(queued.value);
        }

        string jsonData = JsonUtility.ToJson(effectData);
        return jsonData;
    }

    public class EffectsDataParam : Param
    {
        public DataManager dataManager;
    }
}

[Serializable]
public class SoAndInt
{
    public EffectSO effectSo;
    public int value;
    public SoAndInt(EffectSO so, int value)
    {
        effectSo = so;
        this.value = value;
    }
        
}
