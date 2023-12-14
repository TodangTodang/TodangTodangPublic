using System;
using UnityEngine;
using UnityEngine.UI;
using static Strings;

public class UI_StartScene : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _creditButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private float _fadeTime;

    private bool _isInit = false;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private SceneManagerEx _sceneManagerEx;
    private SoundManager _soundManager;

    public void Awake()
    {
        #region Null Exception
        Debug.Assert(_startButton != null, "Null Exception : _startButton");
        Debug.Assert(_settingButton != null, "Null Exception : _settingButton");
        Debug.Assert(_creditButton != null, "Null Exception : _creditButton");
        Debug.Assert(_exitButton != null, "Null Exception : _exitButton");
        #endregion

        _startButton.onClick.AddListener(OnClickStartButton);
        _settingButton.onClick.AddListener(OnSettingButton); 
        _creditButton.onClick.AddListener(OnClickCreditButton);
        _exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isInit = true;
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : SceneManagerEx");
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
    }

    public void OnSettingButton()
    {
        if (!_isInit) Init();

        // 모바일
        // UI_GameSettings uiGameSettings = _uiManager.GetUIComponent<UI_GameSettings>();
        // Debug.Assert(uiGameSettings != null, "Null Exception : UI_GameSettings");
        // uiGameSettings.OpenUI();

        UI_GameSettingsPC uiGameSettingsPC = _uiManager.GetUIComponent<UI_GameSettingsPC>();
        Debug.Assert(uiGameSettingsPC != null, "Null Exception : UI_GameSettingsPC");
        uiGameSettingsPC.OpenUI();
    }

    public void OnClickStartButton()
    {
        if (!_isInit) Init();

        UI_ScreenFader uiScreenFader = _uiManager.GetUIComponent<UI_ScreenFader>();
        Debug.Assert(uiScreenFader != null, "Null Exception : UI_ScreenFader");
        uiScreenFader.StartFadeBoxFadeOut(_fadeTime, LoadNextScene);
        _soundManager.Play(Strings.Sounds.UI_BUTTON); 
    }
    
    public void OnClickCreditButton()
    {
        if (!_isInit) Init();

        UI_Credit uiCredit = _uiManager.GetUIComponent<UI_Credit>();
        Debug.Assert(uiCredit != null, "Null Exception : UI_GameSettings");
        uiCredit.OpenUI();
    }
    
    public void OnClickExitButton()
    {
        if (!_isInit) Init();

        _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: "게임을 종료하시겠습니까?"
                , confirmCallback: ConfirmToExit
                , cancelCallback: () => {}
                )
            );
    }

    private void LoadNextScene()
    {
        if (!_isInit) Init();

        PlayerData playerData = _gameManager.GetPlayerData();
        Debug.Assert(playerData != null, "Null Exception : PlayerData");

        if (playerData.IsNotFirstPlay)
        {
            Enums.PlayerEndingState state = playerData.EndingState;
            switch (state)
            {
                case Enums.PlayerEndingState.ContinuePlaying:
                    _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.HomeScene);
                    break;
                case Enums.PlayerEndingState.BankruptcyEnding:
                    _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.BankruptcyScene);
                    break;
                case Enums.PlayerEndingState.GameOverEnding:
                    _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.GameOverScene);
                    break;
                case Enums.PlayerEndingState.GameClearEnding:
                    _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.GameClearScene);
                    break;
                default:
                    _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.HomeScene);
                    break;
            }
        }
        else
        {
            playerData.IsNotFirstPlay = true;
            _sceneManagerEx.LoadScene(Scenes.PrologueScene);
        }
    }

    private void ConfirmToExit()
    {
        if (!_isInit) Init();

        _gameManager.ApplicationExit();
    }
}
