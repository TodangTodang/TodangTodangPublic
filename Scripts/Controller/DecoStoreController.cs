using System;
using System.Collections.Generic;
using UnityEngine;

public class DecoStoreController : MonoBehaviour
{
    [SerializeField] private UI_DecoStore _uiDecoStore;

    private bool _isInit = true;
    private bool _isInitManagers = false;

    private GameManager _gameManager;
    private DataManager _dataManager;
    private EffectManager _effectManager;
    private PlayerData _playerData;
    private DecoStoreData _decoStoreData;
    private List<StoreDecorationInfoData> _storeDecoList;

    private void Start()
    {
        if (!_isInitManagers) InitManagers();
        _uiDecoStore.OnOpenDecoStore += RefreshData;
        _uiDecoStore.OnBuyStoreDeco += BuyStoreDeco;
        RefreshData();
    }

    private void InitManagers()
    {
        _isInitManagers = true;

        _gameManager = GameManager.Instance;
        _dataManager = DataManager.Instance;
        _effectManager = EffectManager.Instance;

        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_effectManager != null, "Null Exception : EffectManager");

        _playerData = _gameManager.GetPlayerData();
        _decoStoreData = _gameManager.GetDecoStore();

        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
        Debug.Assert(_decoStoreData != null, "Null Exception : DecoStoreData");
    }

    public void RefreshData()
    {
        if (!_isInitManagers) InitManagers();

        _storeDecoList = _decoStoreData.GetAllStoreDecoData();
        _storeDecoList.Sort((a, b) =>
        {
            if (!a.IsSoldOut && !b.IsSoldOut)
            {
                return (a.DefaultData.Price < b.DefaultData.Price) ? -1 : 1;
            } else if (a.IsSoldOut && b.IsSoldOut)
            {
                return 0;
            } else
            {
                return (!a.IsSoldOut) ? -1 : 1;
            }
        });
        _uiDecoStore.UpdatePlayerMoney(_playerData.Money, _isInit);
        _uiDecoStore.RefreshUI(_storeDecoList);
        if (_isInit) _isInit = false;
    }

    private void BuyStoreDeco(int id)
    {
        if (!_isInitManagers) InitManagers();

        _decoStoreData.UpdateStoreDecoSoldOutState(id);
        StoreDecorationInfoSO item = _decoStoreData.GetStoreDecoData(id)?.DefaultData;
        if (item != null) _playerData.BuyStoreDecoration(item);
        _effectManager.AddEffect(item.Effect, -1);
        _dataManager.SaveAllData();

        RefreshData();
    }

    private void OnDisable()
    {
        _isInit = true;
    }
}
