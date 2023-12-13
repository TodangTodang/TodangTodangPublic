using System;
using System.Collections.Generic;
using UnityEngine;

public class DecoStoreData : Savable
{
    private List<StoreDecorationInfoData> _storeDecoDatas;

    private DataManager _dataManager;

    public event Action<int> OnDecorationBought; 

    public DecoStoreData(List<StoreDecorationInfoData> storeDecoDatas) : base()
    {
        _storeDecoDatas = storeDecoDatas;
    }

    public DecoStoreData(DecoStoreSaveData saveData)
    {
        
    }

    public DecoStoreData() : base()
    {
        
    }

    public List<StoreDecorationInfoData> GetAllStoreDecoData()
    {
        return _storeDecoDatas;
    }

    public StoreDecorationInfoData GetStoreDecoData(int id)
    {
        for (int i = 0; i < _storeDecoDatas.Count; i++)
        {
            if (id == _storeDecoDatas[i].DefaultData.ID)
            {
                return _storeDecoDatas[i];
            }
        }
        return null;
    }

    public void UpdateStoreDecoSoldOutState(int id)
    {
        for (int i = 0; i < _storeDecoDatas.Count; i++)
        {
            if (id == _storeDecoDatas[i].DefaultData.ID)
            {
                _storeDecoDatas[i].IsSoldOut = true;
                OnDecorationBought?.Invoke(id); 
                break;
            }
        }
    }

    public override void Init(string json, Param saveParam = null)
    {
        DecoStoreSaveData saveData = JsonUtility.FromJson<DecoStoreSaveData>(json);
        DecoStoreDataParam decoStoreDataParam = saveParam as DecoStoreDataParam;
        
        _storeDecoDatas = saveData.StoreDecorationInfoDatas;
        
        SOCheckUtil.RecheckSO(_storeDecoDatas);
        
        if (_dataManager == null) _dataManager = DataManager.Instance;
        Debug.Assert(_dataManager != null,"Null Exception : DataManager");
        SOCheckUtil.CheckNewDefaultData<StoreDecorationInfoSO,StoreDecorationInfoData>(decoStoreDataParam._storeDecoDatas,_storeDecoDatas);
    }

    public override string GetJsonData()
    {
        DecoStoreSaveData decoData = new DecoStoreSaveData();
        decoData.StoreDecorationInfoDatas = GetAllStoreDecoData();
    
        string jsonData = JsonUtility.ToJson(decoData);
        return jsonData;
    }
    
    public class DecoStoreDataParam : Param
    {
        public StoreDecorationInfoSO[] _storeDecoDatas;
    }

}

