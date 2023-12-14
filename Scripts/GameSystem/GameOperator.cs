using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameOperator : MonoBehaviour
{
    [Header("손님 생성기")] 
    [SerializeField] private GameObject _customerGeneratorPrefab;
    [SerializeField] private Vector3 _customerGeneratorInitPos;
    [SerializeField] private bool _enableManualCustomerSpawn;

    [Header("손님 생성 수")] 
    [SerializeField] private int _baseCustomerCount;
    [SerializeField] private int _maxCustomerCount;
    [SerializeField] private float[] _rushHourTime;
    
    [Header("게임 종료 후 일정 대기 시간")]
    [SerializeField] private float _gameEndWait;
    private int _currentExitedCustomerCount;
    
    [Header("전체시간 비율")]
    [SerializeField] private float _totalTimeRate; 
    
    private CustomerGenerator _customerGenerator;
    private GameSceneHUDController _gameSceneHUDController;

    private SalesData _salesData;
    private GameManager _gameManager;
    private EffectManager _effectManager;
    private UIManager _uiManager;
    private SoundManager _soundManager;
    private PlayerData _playerData;

    private int _customCustomerCount = 0;
    private int _rushHourIdx = 0;
    private float _todayTime = 0;
    private float _currentTime = 0;
    private bool _isFlash = false;
    public event Action OnGameFinishCallback;
    
    void Start()
    {
        Init();
        if(!_enableManualCustomerSpawn)
            StartClock();
    }

    private IEnumerator SpendTime()
    {
        while (_todayTime > _currentTime && _salesData.TotalCustomerCount > _currentExitedCustomerCount)
        {
            _currentTime += Time.deltaTime;
            float timeRate = GetTime();
            _gameSceneHUDController.ChangeTime(timeRate);
            CheckRushHour();

            if (_todayTime - _currentTime <= 10f && !_isFlash)
            {
                _gameSceneHUDController.FlashSpendDay(.5f);
                _isFlash = true;
            }

            if (_todayTime - _currentTime <= 5f && !_soundManager.IsPlaying(Strings.Sounds.UI_TIME))
            {
                _soundManager.Play(Strings.Sounds.UI_TIME);
            }

            yield return null;
        }
        
        FinishGame();
    }

    private void CheckRushHour()
    {
        Debug.Assert(_rushHourTime.Length == 4, "rushHourTime은 무조건 4개여야합니다. 시작, 끝, 시작, 끝");
        if (_rushHourIdx >= 4 || _enableManualCustomerSpawn)
            return;
        float rushHourtime = _rushHourTime[_rushHourIdx];
        float currentTimeRate = GetTime();
        if(currentTimeRate > rushHourtime)
        {
            if (_rushHourIdx % 2 == 0)
            {
                _customerGenerator.StartRushHour();
                Debug.Log("rushHour 시작");
            }
            else
            {
                _customerGenerator.EndRushHour();
                Debug.Log("rushHour 끝");
            }
            _rushHourIdx++;
        }
    }

    public void Init()
    {
        _gameManager = GameManager.Instance;
        _effectManager = EffectManager.Instance;
        if(!_effectManager.isInit)
            _effectManager.Init();
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        CheckValidMember();
        InitPlayData();
        InitGameSceneHUD();
        InitCustomerGenerator();
    }

    private void CheckValidMember()
    {
        Debug.Assert(_gameManager,$"_gameManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_effectManager,$"_effectManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_uiManager, $"_uiManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_soundManager, $"_soundManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_customerGeneratorPrefab, $"_customerGeneratorPrefab{Strings.DebugLog.NOT_ALLOCATE_IN_INSPECTOR}");
    }

    private void InitCustomerGenerator()
    {
        GameObject generatorObj = Instantiate(_customerGeneratorPrefab, _customerGeneratorInitPos, Quaternion.identity);
        _customerGenerator= generatorObj.GetComponent<CustomerGenerator>();
        Debug.Assert(_customerGenerator,$"CustomerGenerator{Strings.DebugLog.NOT_ALLOCATE_IN_INSPECTOR}");
        
        _customerGenerator.Init(OnCustomerIn,OnCustomerExit,OnBoughtCheck,_enableManualCustomerSpawn);
        _currentExitedCustomerCount = 0;
        _customerGenerator.StartSpawn();
    }

    public void SpawnCustomer()
    {
        _customerGenerator.SpawnCustomCustomer();
    }

    public void EnableAutoCustomerGenerate()
    {
        _customerGenerator._isCustomMode = false;
        _customerGenerator.StartSpawn();
    }
    
    public void SetTotalCustomerCount(int count)
    {
        _customCustomerCount = count;
    }

    public void StartClock()
    {
        StartCoroutine(SpendTime());
    }

    private void InitPlayData()
    {
        _playerData = _gameManager.GetPlayerData();
        Debug.Assert(_playerData != null, $"_playerData {Strings.DebugLog.INIT_PROBLEM}");

        _salesData = new SalesData();
        _salesData.SelledFoods = new Dictionary<IngredientInfoSO, int>();
        _salesData.ExpiredFoods = new Dictionary<IngredientInfoSO, int>();
        if (_enableManualCustomerSpawn && _customCustomerCount > 0)
        {
            _salesData.TotalCustomerCount = _customCustomerCount;
        }
        else
        {
            _salesData.TotalCustomerCount = GetCustomerCount();
        }

        Debug.Log($"오늘 손님 : {_salesData.TotalCustomerCount}");
   
    }

    private void InitGameSceneHUD()
    {
        DataManager dataManager = DataManager.Instance;

        PlayerData playerData = _gameManager.GetPlayerData(); 
        UI_GameSceneHUD hud = _uiManager.GetUIComponent<UI_GameSceneHUD>();
        CustomerDataSO dataSo = dataManager.GetDefaultData<CustomerDataSO>("BaseCustomerData");

        Debug.Assert(dataManager,"dataManager가 없습니다");

        _gameSceneHUDController = new GameSceneHUDController();
        
        hud.Init(FinishGame);
        _gameSceneHUDController.Init(hud);
        _gameSceneHUDController.SetDay(playerData.Date);
        _gameSceneHUDController.SetEarnMoney(0);
        _gameSceneHUDController.ChangeTime(0);
        
        _todayTime = _salesData.TotalCustomerCount * dataSo.BaseEndurance * _totalTimeRate;
        Debug.Log($"{_todayTime}초의 하루입니다");
         
        _gameSceneHUDController.SetLeftCustomerCount(_salesData.TotalCustomerCount, true);
    }

    private void OnBoughtCheck(CustomerEmotionType satisfy,List<IngredientInfoSO> orderedFood, int totalEarn)
    {
        ResourceManager resourceManager = ResourceManager.Instance;
        int totalFoodCost = 0;
        
        Debug.Assert(resourceManager,"resourceManager가 초기화 되지 않았습니다.");
        
        switch (satisfy)
        {
            case CustomerEmotionType.Perfect :
                ++_salesData.PerfectCustomerCount;
                break;
            case CustomerEmotionType.Great :
                ++_salesData.GreatCustomerCount;
                break;
            case CustomerEmotionType.SoSo :
                ++_salesData.SoSoCustomerCount;
                break;
            case CustomerEmotionType.Angry :
                ++_salesData.AngryCustomerCount;
                break;
        }


        foreach (var food in orderedFood)
        {
            Debug.Log(food.Name);
            if (_salesData.SelledFoods.TryGetValue(food, out int value))
                _salesData.SelledFoods[food] = value + 1;
            else
                _salesData.SelledFoods[food] = 1;
        }

        _salesData.EarnMoney += totalEarn;

        _gameSceneHUDController.SetEarnMoney(_salesData.EarnMoney);
        
        Debug.Log($"완전 만족 수 {_salesData.PerfectCustomerCount}");
        Debug.Log($"적당히 만족 수 {_salesData.GreatCustomerCount}");
        Debug.Log($"만족 수 {_salesData.SoSoCustomerCount}");
        Debug.Log($"불만족 수 {_salesData.AngryCustomerCount}");
    }

    private float GetTime()
    {
        return _currentTime/_todayTime;
    }

    private void OnCustomerIn()
    {
        ++_salesData.EnterCustomerCount;
        _gameSceneHUDController.SetLeftCustomerCount(_salesData.TotalCustomerCount-_salesData.EnterCustomerCount);
        Debug.Log(_salesData.EnterCustomerCount);
        if (_salesData.TotalCustomerCount <= _salesData.EnterCustomerCount)
        {
            _customerGenerator.StopSpawn();
        }
    }

    public void FinishGame()
    {
        SoundManager.Instance.Stop();
        SoundManager.Instance.Play("UI/GameEnd");
        _customerGenerator.StopSpawn();
        StartCoroutine(GameFinishing());
    }

    public IEnumerator GameFinishing()
    {
        yield return CoroutineTime.GetWaitForSecondsRealtime(_gameEndWait);
        
        UI_StoreClosed StoreClosed = _uiManager.GetUIComponent<UI_StoreClosed>();
        StoreClosed.OpenUI(false, true);

        yield return CoroutineTime.GetWaitForSecondsRealtime(3f);
        StoreClosed.CloseUI(false, true);

        UI_ResultPanel result = _uiManager.GetUIComponent<UI_ResultPanel>();
        ResultPanelController resultPanelController = result.GetComponent<ResultPanelController>();
        Debug.Assert(resultPanelController, "resultPanelController는 등록되지 않은 컴포넌트 입니다.");

        resultPanelController.Init(ref _salesData);
        result.OpenUI(false);
        result.FadeBackground();

        OnGameFinishCallback?.Invoke();
    }

    private int GetCustomerCount()
    {
        int extraSpawnAmount = (int)_effectManager.GetCustomerEffectRate(CustomerEffectType.CustomerCount);
        int customerNum = _playerData.Date / 3 + _baseCustomerCount;
        int randomAddCustomerCount = _playerData.Star / 10;
        int resultCustomerNum = Mathf.Min(customerNum + Random.Range(0, randomAddCustomerCount+1)+extraSpawnAmount,_maxCustomerCount);
        
        Debug.Log($"{customerNum} + {randomAddCustomerCount} = {resultCustomerNum}, ({extraSpawnAmount})");
        return resultCustomerNum;
    }

    private void OnCustomerExit()
    {
        ++_currentExitedCustomerCount;
        Debug.Log($"잘가 {_currentExitedCustomerCount} / {_salesData.TotalCustomerCount}");
        // if (_currentExitedCustomerCount == _playData.TotalCustomerCount)
        // {
        //     FinishGame();
        // }
    }
    
    #if UNITY_EDITOR
    public void ChangeCurrentTime(float time)
    {
        _currentTime = time * _todayTime;
    }
    #endif
}
