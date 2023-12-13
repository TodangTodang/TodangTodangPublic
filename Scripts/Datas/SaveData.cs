
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Savable
{
    private DataManager _dataManager;

    public Savable()
    {
        _dataManager = DataManager.Instance;
#if UNITY_EDITOR
        Debug.Assert(_dataManager, $"dataManager {Strings.DebugLog.INIT_PROBLEM} ");
#endif

        _dataManager.RegistSaveData(this);
    }

    public abstract void Init(string json, Param saveParam = null);
    public abstract string GetJsonData();

    ~Savable()
    {
        _dataManager.CancelRegistSaveData(this);
    }
}

public abstract class SaveData
{
    
}

public abstract class Param
{
    
}
