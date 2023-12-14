using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private static string b = "BFF3CBAF81A079729C11A25180C8E8069F8304B05262FC80D22F6D7DED6E7166";
    private static readonly string a = b.Substring(0, 128 / 8);
    private Dictionary<Type, Dictionary<string, ScriptableObject>> _defaultDataBank;

    private bool _isInited = false;

    public bool isInited
    {
        get
        {
            return _isInited;
        }
    }
    private string _playerDataSavePath;
    private string _marketDataSavePath;
    private string _decoStoreDataPath;
    private string _effectsDataPath;
    private string _newsDataPath;

    private const string _playerDataSaveName = "PlayerDataSave.sav";
    private const string _marketDataSaveName = "MarketDataSave.sav";
    private const string _decoStoreDataName = "DecoStoreDataSave.sav";
    private const string _effectsDataName = "EffectSave.sav";
    private const string _newsDataName = "NewsSave.sav";

    public bool isCorrupted { get; private set; } = false;


    #region InitMethods

    private Dictionary<Type, string> _filePathDic;
    private HashSet<Savable> _saveDatas;


    private GameSettingsData gameSettingsData;
    protected void Awake()
    {
        Init();
    }

    public void Init()
    {
        _playerDataSavePath = Path.Combine(Application.persistentDataPath, _playerDataSaveName);
        _marketDataSavePath = Path.Combine(Application.persistentDataPath, _marketDataSaveName);
        _decoStoreDataPath = Path.Combine(Application.persistentDataPath, _decoStoreDataName);
        _effectsDataPath = Path.Combine(Application.persistentDataPath, _effectsDataName);
        _newsDataPath = Path.Combine(Application.persistentDataPath, _newsDataName);
        InitDefaultData();
        _isInited = true;

        _filePathDic = new Dictionary<Type, string>();
        _filePathDic.Add(typeof(PlayerData), _playerDataSavePath);
        _filePathDic.Add(typeof(MarketData), _marketDataSavePath);
        _filePathDic.Add(typeof(DecoStoreData), _decoStoreDataPath);
        _filePathDic.Add(typeof(EffectsData), _effectsDataPath);
        _filePathDic.Add(typeof(NewsSystem), _newsDataPath);

        _saveDatas = new HashSet<Savable>();

    }

    private void InitDefaultData()
    {
        _defaultDataBank = new Dictionary<Type, Dictionary<string, ScriptableObject>>();

        ScriptableObject[] datas = Resources.LoadAll<ScriptableObject>("ScriptableDatas");

        foreach (var scriptableData in datas)
        {
            AddDefaultData(scriptableData);
        }
    }

    private void AddDefaultData(ScriptableObject scriptableData)
    {
        Type type = scriptableData.GetType();

        Dictionary<string, ScriptableObject> dic;

        // EffectSO는 EffectSO의 통합관리의 필요로로 추가된 부분
        // EffectSO로 저장하고 불러오는 부분으로 추가된 예외처리
        if (scriptableData is EffectSO)
        {
            Dictionary<string, ScriptableObject> effectDic;
            Type effectType = typeof(EffectSO);
            if (!_defaultDataBank.TryGetValue(effectType, out effectDic))
            {
                effectDic = new Dictionary<string, ScriptableObject>();
                _defaultDataBank.Add(effectType, effectDic);
            }
            effectDic.TryAdd(scriptableData.name, scriptableData);
        }

        if (!_defaultDataBank.TryGetValue(type, out dic))
        {
            dic = new Dictionary<string, ScriptableObject>();
            _defaultDataBank.Add(type, dic);
        }
        dic.TryAdd(scriptableData.name, scriptableData);
    }
    #endregion

    #region UserMethod

    public U GetDefaultData<U>(string name) where U : ScriptableObject
    {
        U result = null;
        if (_defaultDataBank.TryGetValue(typeof(U), out Dictionary<string, ScriptableObject> dic))
        {
            if (dic.TryGetValue(name, out ScriptableObject value))
            {
                result = value as U;
            }
            else
            {
                Debug.Log($"{typeof(U)} : {name}은 Resources에 등록되지 않은 데이터 입니다");
            }
        }
        else
        {
            Debug.Log($"{nameof(U)}로 생성된 저장공간이 없습니다");
        }
        return result;
    }

    public U[] GetDefaultDataArray<U>() where U : ScriptableObject
    {
        U[] result = null;
        if (_defaultDataBank.TryGetValue(typeof(U), out Dictionary<string, ScriptableObject> dic))
        {
            result = new U[dic.Count];
            int i = 0;
            foreach (ScriptableObject value in dic.Values)
            {
                result[i] = value as U;
                ++i;
            }
        }
        else
        {
            Debug.Log($"{nameof(U)}로 생성된 저장공간이 없습니다");
        }

        return result;

    }

    #endregion

    #region SaveAndLoadMethod

    #region SaveAndLoad

    public void RegistSaveData(Savable saveData)
    {
        _saveDatas.Add(saveData);
    }

    public void CancelRegistSaveData(Savable saveData)
    {
        _saveDatas.Remove(saveData);
    }

    public void SaveAllData()
    {
        foreach (Savable data in _saveDatas)
        {
            Type type = data.GetType();
            if (_filePathDic.TryGetValue(type, out string filePath))
            {
                string jsonData = data.GetJsonData();
                Saving(jsonData, filePath);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("type에 맞는 filePath가 없습니다. 신경써주세요");
#endif
            }

        }
    }

    public void Load<T>(out T data) where T : Savable, new()
    {
        Type dataType = typeof(T);
        data = null;
        string jsonData = null;
        if (_filePathDic.TryGetValue(dataType, out string filePath) && File.Exists(filePath))
        {
            jsonData = Loading(filePath, out bool needEncrypt);

            if (jsonData == string.Empty || isCorrupted)
            {
                GetDefaultData<T>(dataType, out data);
            }
            else
            {
                data = new T();
                Param param = GetParam<T>();
                data.Init(jsonData, param);

            }
            if (needEncrypt)
            {
                Saving(jsonData, filePath);
            }
        }
        else
        {
#if UNITY_EDITOR
            if (filePath == null)
                Debug.LogWarning("type에 맞는 filePath가 없습니다. 신경써주세요");
#endif
            GetDefaultData<T>(dataType, out data);
        }
    }

    public void LoadSaveData<U>(out U data) where U : SaveData
    {
        data = null;
        Type saveDataType = typeof(U);
        if (_filePathDic.TryGetValue(saveDataType, out string filePath) && File.Exists(filePath))
        {
            string jsonData = Loading(filePath, out bool needEncrypt);

            if (jsonData != string.Empty && !isCorrupted)
            {
                data = JsonUtility.FromJson<U>(jsonData);
            }
            if (needEncrypt)
            {
                Saving(jsonData, filePath);
            }
        }

    }

    private void GetDefaultData<T>(Type dataType, out T data) where T : Savable
    {
        data = null;
        if (dataType == typeof(PlayerData))
        {
            data = MakeDefaultPlayerData() as T;
        }
        else if (dataType == typeof(MarketData))
        {
            data = MakeDefaultMarketData() as T;
        }
        else if (dataType == typeof(DecoStoreData))
        {
            data = MakeDefaultDecoStoreData() as T;
        }
        else if (dataType == typeof(NewsSystem))
        {
            data = new NewsSystem() as T;
        }
        else if (dataType == typeof(EffectsData))
        {
            data = new EffectsData() as T;
        }
    }

    private Param GetParam<T>() where T : Savable
    {
        Param data = null;
        Type dataType = typeof(T);
        if (dataType == typeof(MarketData))
        {
            MarketData.MarketDataParam param = new MarketData.MarketDataParam();
            IngredientInfoSO[] ingredientInfoSos = GetDefaultDataArray<IngredientInfoSO>();
            param.IngredientInfoSos = ingredientInfoSos;
            data = param;
        }
        else if (dataType == typeof(DecoStoreData))
        {
            DecoStoreData.DecoStoreDataParam param = new DecoStoreData.DecoStoreDataParam();
            StoreDecorationInfoSO[] storeDecorationInfoSos = GetDefaultDataArray<StoreDecorationInfoSO>();
            param._storeDecoDatas = storeDecorationInfoSos;
            data = param;
        }
        else if (dataType == typeof(EffectsData))
        {
            EffectsData.EffectsDataParam param = new EffectsData.EffectsDataParam();
            param.dataManager = this;
            data = param;
        }

        return data;
    }

    private void Saving(string data, string path)
    {
        using (StreamWriter sr = new StreamWriter(path))
        {
            string encryptingData = EncryptData(data);
            string versionAddedData = $"{Strings.SaveData.SAVE_VERSION}|{encryptingData}";
            string hashString = versionAddedData.GetHashCode().ToString();
            sr.Write($"{Strings.SaveData.SAVE_VERSION}|{encryptingData}|{hashString}");
        }
    }

    private string Loading(string path, out bool needEncrypt)
    {
        string data = string.Empty;
        needEncrypt = false;
        using (StreamReader sr = new StreamReader(path))
        {
            string encrypdata = sr.ReadToEnd();
            // 추후에 버전 체크를 할 수 있게 변형할 수 있음.
            if (encrypdata.StartsWith(Strings.SaveData.SAVE_VERSION))
            {
                string[] strIntegrityCheckSplit = encrypdata.Split("|");
                if (strIntegrityCheckSplit.Length != 3)
                {
                    isCorrupted = true;
                    Debug.LogWarning("데이터의 이상");
                    return string.Empty;
                }
                else
                {
                    string hashString = $"{strIntegrityCheckSplit[0]}|{strIntegrityCheckSplit[1]}";

                    if (int.TryParse(strIntegrityCheckSplit[2], out int hash) && hash == hashString.GetHashCode())
                    {
                        data = DecryptData(strIntegrityCheckSplit[1]);
                    }
                    else
                    {
                        isCorrupted = true;
                        Debug.LogWarning("데이터의 이상");
                        return string.Empty;
                    }
                }
            }
            else if (encrypdata.StartsWith("{"))
            {
                data = encrypdata;
                needEncrypt = true;
            }
            else
            {
                isCorrupted = true;
                Debug.LogWarning("데이터의 이상");
                return string.Empty;
            }

        }
        return data;
    }

    #endregion

    #region Encrypte Decrypt
    private string EncryptData(string data)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(data);

        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 128;

        MemoryStream memoryStream = new MemoryStream();
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(a));
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();
        return encryptString;
    }

    private string DecryptData(string data)
    {
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        string plainString = String.Empty;
        try
        {
            byte[] encryptBytes = Convert.FromBase64String(data);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 256;
            rijndaelManaged.BlockSize = 128;

            memoryStream = new MemoryStream(encryptBytes);
            ICryptoTransform decryptor =
                rijndaelManaged.CreateDecryptor(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(a));
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainBytes = new byte[encryptBytes.Length];

            int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);
            cryptoStream.Close();
            memoryStream.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            if (cryptoStream != null)
            {
                cryptoStream.Clear();
                cryptoStream.Close();
            }
            if (memoryStream != null)
            {
                memoryStream.Close();
            }

        }
        return plainString;
    }

    #endregion

    #region MakeDefaultData

    private PlayerData MakeDefaultPlayerData()
    {
        PlayerData playerData = new PlayerData();
        //데이터 췍 (저장데이터가 있다면 해당 데이터, 아니라면 새로 생성)
        // 기본 데이터 생성도 여기서 하면 되겠구만.
        // 재료 데이터 추가
        playerData.EndDay();
        playerData.UpdateStar(10);
        playerData.UpdateMoney(5000);
        playerData.UpdateEndingState(Enums.PlayerEndingState.ContinuePlaying);
        playerData.UpdateDayCycleState(Enums.PlayerDayCycleState.StartStore);
        playerData.UpdateNeedHelp(false);
        playerData.IsNotFirstPlay = false;
        playerData.UpdateCompleteUpgradeTutorial(false);

        var RiceFlour = GetDefaultData<IngredientInfoSO>("RiceFlour");
        List<IngredientInfoData> list = playerData.GetInventory<IngredientInfoData>();
        IngredientInfoData riceFlourData = new IngredientInfoData(RiceFlour, 10);
        list.Add(riceFlourData);



        //레시피 데이터 추가
        var recipeDefaults = GetDefaultDataArray<RecipeInfoSO>();
        var recipeList = playerData.GetInventory<RecipeInfoData>();
        foreach (RecipeInfoSO recipe in recipeDefaults)
        {
            RecipeInfoData data = new RecipeInfoData(GetDefaultData<RecipeInfoSO>(recipe.name));
            recipeList.Add(data);
        }

        recipeList.Sort((x, y) => x.DefaultData.ID - y.DefaultData.ID);
        recipeList[0].Level = 1;


        //주방기구 데이터 추가
        var utensilDefaults = GetDefaultDataArray<KitchenUtensilInfoSO>();
        var utensilList = playerData.GetInventory<KitchenUtensilInfoData>();

        for (int i = 0; i < utensilDefaults.Length; i++)
        {
            KitchenUtensilInfoData data = new KitchenUtensilInfoData(utensilDefaults[i]);
            utensilList.Add(data);
            if (utensilDefaults[i].ID == 4 || utensilDefaults[i].ID == 1 || utensilDefaults[i].ID == 2)
                data.Level = 0;
        }

        return playerData;
    }

    private MarketData MakeDefaultMarketData()
    {            
        IngredientInfoSO[] ingredientInfoSos = GetDefaultDataArray<IngredientInfoSO>();
        MarketData marketSystem;
        
        marketSystem = new MarketData(ingredientInfoSos);

        Dictionary<string, bool> isSellable = marketSystem.GetIsSellableDatas();
        for (int i = 0; i < ingredientInfoSos.Length; i++)
        {
            isSellable.Add(ingredientInfoSos[i].name, false);
        }

        Dictionary<string, int> ingredientPrices = marketSystem.GetIngredientPrices();
        for (int i = 0; i < ingredientInfoSos.Length; i++)
        {
            ingredientPrices.Add(ingredientInfoSos[i].name, ingredientInfoSos[i].BasePrice);
        }

        return marketSystem;
    }

    public void SaveGameSettings(GameSettingsData gameSettingsData)
    {
        this.gameSettingsData = gameSettingsData;
        PlayerPrefs.SetFloat(Strings.Prefs.BGMVOLUME_KEY, gameSettingsData.BgmVolume);
        PlayerPrefs.SetFloat(Strings.Prefs.EFFECTVOLUME_KEY, gameSettingsData.EffectVolume);
        PlayerPrefs.SetInt(Strings.Prefs.TEXTUREQUALITY_KEY, (int)gameSettingsData.TextureQuality);
        PlayerPrefs.SetInt(Strings.Prefs.FRAMERATE_KEY, gameSettingsData.Framerate);

        PlayerPrefs.Save(); 
    }

    private DecoStoreData MakeDefaultDecoStoreData()
    {
        DecoStoreData decoStoreData;
        StoreDecorationInfoSO[] dataSo = GetDefaultDataArray<StoreDecorationInfoSO>();
        List<StoreDecorationInfoData> datas = new List<StoreDecorationInfoData>(dataSo.Length);
        for (int i = 0; i < dataSo.Length; i++)
        {
            datas.Add(new StoreDecorationInfoData(dataSo[i]));
        }
        decoStoreData = new DecoStoreData(datas);
        return decoStoreData;
    }

    #endregion

    #region GameSettingSave

    public GameSettingsData LoadGameSettings()
    {
        if (gameSettingsData != null)
        {
            return gameSettingsData;
        }
        if (!PlayerPrefs.HasKey(Strings.Prefs.BGMVOLUME_KEY))
        {
            SetDefaultGameSettings();
        }
        else
        {
            float bgmVolume = PlayerPrefs.GetFloat(Strings.Prefs.BGMVOLUME_KEY);
            float effectVolume = PlayerPrefs.GetFloat(Strings.Prefs.EFFECTVOLUME_KEY);
            int textureQuality = PlayerPrefs.GetInt(Strings.Prefs.TEXTUREQUALITY_KEY);
            int framerate = PlayerPrefs.GetInt(Strings.Prefs.FRAMERATE_KEY);
            gameSettingsData = new GameSettingsData(bgmVolume, effectVolume, (Enums.TextureQuality)textureQuality, framerate);
        }

        return gameSettingsData;
    }

    public void SetDefaultGameSettings()
    {
        float bgmVolume = 0.5f;
        float effectVolume = 0.5f;
        int textureQuality = 1;
        int framerate = 30;

        gameSettingsData = new GameSettingsData(bgmVolume, effectVolume, (Enums.TextureQuality)textureQuality, framerate);
        PlayerPrefs.SetFloat(Strings.Prefs.BGMVOLUME_KEY, bgmVolume);
        SoundManager.Instance.SetVolume(bgmVolume, Enums.AudioType.Bgm);
        PlayerPrefs.SetFloat(Strings.Prefs.EFFECTVOLUME_KEY, effectVolume);
        SoundManager.Instance.SetVolume(effectVolume, Enums.AudioType.Effect);
        PlayerPrefs.SetInt(Strings.Prefs.TEXTUREQUALITY_KEY, textureQuality);
        PlayerPrefs.SetInt(Strings.Prefs.FRAMERATE_KEY, framerate);
        QualitySettings.globalTextureMipmapLimit = textureQuality;
        Application.targetFrameRate = framerate;
    }

    #endregion

    #region DeleteFile


    public void DeletePlaySaveDataAll()
    {
        if (File.Exists(_playerDataSavePath))
        {
            File.Delete(_playerDataSavePath);
        }
        if (File.Exists(_marketDataSavePath))
        {
            File.Delete(_marketDataSavePath);
        }
        if (File.Exists(_decoStoreDataPath))
        {
            File.Delete(_decoStoreDataPath);
        }
        if (File.Exists(_effectsDataPath))
        {
            File.Delete(_effectsDataPath);
        }
        if (File.Exists(_newsDataPath))
        {
            File.Delete(_newsDataPath);
        }

        isCorrupted = false;
        GameManager.Instance.EraseAllData();
        EffectManager.Instance.EraseAllData();
    }

    public void DeleteDataAll()
    {
        DeletePlaySaveDataAll();
        SetDefaultGameSettings();
    }
    #endregion

    #endregion
}


#region SaveDataClass
[Serializable]
public class PlayerSaveData
{
    public int Star;
    public int Money;
    public int Date;
    public Enums.PlayerDayCycleState DayCycleState;
    public Enums.PlayerEndingState EndingState;
    public List<IngredientInfoData> IngredientDatas;
    public List<RecipeInfoData> RecipeInfoDatas;
    public List<KitchenUtensilInfoData> KitchenUtensilInfoDatas;
    public List<string> StoreDecorationDatas;
    public List<string> NewUnLockedRecipeList;
    public bool isNeedHelp;
    public bool isFirstPlay;
    public bool IsCompleteUpgradeTutorial;
}

[Serializable]
public class MarketSaveData
{
    public List<string> IsSellableStrs;
    public List<bool> IsSellableValues;
    public List<string> IngredientPriceStrs;
    public List<int> IngredientPriceValues;
}

[Serializable]
public class DecoStoreSaveData
{
    public List<StoreDecorationInfoData> StoreDecorationInfoDatas;
}

[Serializable]
public class EffectSaveData : SaveData
{
    public List<string> IngredientEffects;
    public List<string> CustomerEffects;
    public List<int> IngredientEffectRemainDays;
    public List<int> CustomerEffectRemainDays;
    public List<string> EffectQueueSOString;
    public List<int> EffectQueueValue;
}

public class NewsData
{
    public List<bool> completeAchievements;
}

#endregion