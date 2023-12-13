using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    private bool _isInit;
    private Enums.PlayerDayCycleState _dayCycleState;

    private GameInputActions inputActions;

    private DataManager _dataManager;
    private UIManager _uiManager;
    private SceneManagerEx _sceneManagerEx;
    private EffectManager _effectManager;
    private PlayerData _playerData;
    private MarketData _marketSystem;
    private NewsSystem _newsSystem;
    private DecoStoreData _decoStore;
    private HomeScene _homeScene;

    private bool _isOnControlMode;

    private void Awake()
    {
        inputActions = new GameInputActions();
    }

    private void Start()
    {
        _effectManager = EffectManager.Instance;
        _effectManager.Init();

        _uiManager = UIManager.Instance;

        SetCursor();
        AddInputCallback();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Init()
    {
        _isInit = true;
        if (_dataManager == null) _dataManager = DataManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;
        if (_effectManager == null) _effectManager = EffectManager.Instance;
        
        InitData();

    }

    public void InitData()
    {
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : _sceneManagerEx");
        Debug.Assert(_effectManager != null, "Null Exception : EffectManager");

        _dataManager.Load<PlayerData>(out _playerData);
        _dataManager.Load<MarketData>(out _marketSystem);
        _dataManager.Load<NewsSystem>(out _newsSystem);
        _dataManager.Load<DecoStoreData>(out _decoStore);
        EffectManager.Instance.Init();
        
        if (_dataManager.isCorrupted)
        {
            _dataManager.DeletePlaySaveDataAll();
        }
        else
        {
            Debug.Assert(_playerData != null, "Null Exception : _playerData");
            Debug.Assert(_marketSystem != null, "Null Exception : _marketSystem");
            Debug.Assert(_newsSystem != null, "Null Exception : _newsSystem");
            Debug.Assert(_decoStore != null, "Null Exception : _decoStore");
        }
    }

    public void EraseAllData()
    {
        _playerData = null;
        _marketSystem = null;
        _newsSystem = null;
        _decoStore = null;
    }

    public PlayerData GetPlayerData()
    {
        if (!_isInit) Init();
        if (_playerData == null) InitData();
        return _playerData;
    }

    public Enums.PlayerDayCycleState GetDayCycleState()
    {
        if (!_isInit) Init();
        if (_playerData == null) InitData();
        _dayCycleState = _playerData.DayCycleState;
        return _dayCycleState;
    }

    public MarketData GetMarketData()
    {
        if (!_isInit) Init();
        if (_marketSystem == null) InitData();
        return _marketSystem;
    }

    public NewsSystem GetNewsSystem()
    {
        if (!_isInit) Init();
        if (_newsSystem == null) InitData();
        return _newsSystem;
    }

    public DecoStoreData GetDecoStore()
    {
        if (!_isInit) Init();
        if (_decoStore == null) InitData();
        return _decoStore;
    }

    public void SetControlMode(bool isOnControl)
    {
        _isOnControlMode = isOnControl;
    }

    public void ChangeState(Enums.PlayerDayCycleState state)
    {
        if (!_isInit) Init();
        _dayCycleState = state;
        _playerData.UpdateDayCycleState(state);
        if (SceneManagerEx.Instance.CurrentSceneType == Scenes.HomeScene)
        {
            _homeScene = SceneManagerEx.Instance.CurrentScene as HomeScene;
            Debug.Assert(_homeScene != null, "Unknown Scene : _homeScene");
            _homeScene.ChangeDayCycleState(state);
        }
    }

    public void DayEnd()
    {
        if (!_isInit) Init();
        AnalyticsManager.DayEnd(_playerData.Date);
        _playerData.EndDay();
        ChangeState(Enums.PlayerDayCycleState.StartStore);
        _effectManager.SpendDay();
        if (_newsSystem == null)
            _newsSystem = GetNewsSystem();
        _newsSystem.CheckTodaysNews(_playerData.Date);
        _newsSystem.ViewNews();
        _dataManager.SaveAllData();
        TestVersionEnd();
    }

    public void ApplicationExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit(0);
#endif
    }

    private void SetCursor()
    {
        Texture2D image = Resources.Load<Texture2D>("Sprites/Cursor/MoonCursor");

        Cursor.SetCursor(image, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void AddInputCallback()
    {
        inputActions.GameControl.Exit.started += ConductEscapeBehaviour;
    }

    public void ActivateEscapeInput(bool isActivate)
    {
        if (isActivate) inputActions.Enable();
        else inputActions.Disable();
    }

    private void ConductEscapeBehaviour(InputAction.CallbackContext context)
    {
        UI_Base currentUI = _uiManager.GetCurrentUI();
        if (currentUI != null && !_isOnControlMode)
        {
            if (currentUI is UI_Popup) (currentUI as UI_Popup).ClosePopup(Enums.PopupButtonType.Cancel);
            else currentUI.CloseUI();
        }
        else
        {
            OnExit();
        }
    }

    private void OnExit()
    {
        SceneManagerEx sceneManager = SceneManagerEx.Instance;
        if (sceneManager.CurrentSceneType == Scenes.HomeScene)
        {
            _uiManager.GetUIComponent<UI_GameSettings>().OpenUI();
        }
        else
        {
            UIManager.Instance.ShowPopup<UI_DefaultPopup>(
           new PopupParameter(
               content: Strings.GameSettings.EXITBUTTON
               , confirmCallback: ApplicationExit
               , cancelCallback: null
               )
           );
        }
    }

    private void TestVersionEnd()
    {
        if (_playerData.Date > 9)
        {
            SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.TestVersionCompleteScene);
            _dataManager.DeletePlaySaveDataAll();
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
