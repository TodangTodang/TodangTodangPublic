using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PausePanel : UI_Base
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _homeButton;

    public event Action OnUIOpen;
    public event Action OnUIClose;

    private UI_GameSettings _gameSettings;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SoundManager _soundManager;
    private SceneManagerEx _sceneManagerEx;

    private PlayerData _playerData;

    private void Start()
    {
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;
        if (_playerData == null) _playerData = _gameManager.GetPlayerData(); 

        Debug.Assert(_uiManager != null, "Null Exception : _uiManager");
        Debug.Assert(_gameManager != null, "Null Exception : _gameManager");
        Debug.Assert(_soundManager != null, "Null Exception : _soundManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : _sceneManagerEx");

        Debug.Assert(_continueButton != null, "Null Exception : _continueButton");
        Debug.Assert(_settingsButton != null, "Null Exception : _settingsButton");
        Debug.Assert(_homeButton != null, "Null Exception : _homeButton");
        Debug.Assert(_playerData != null, "Null Exception : _playerData");

        Init();
    }

    private void Init()
    {
        _continueButton.onClick.AddListener(() => CloseUI());
        _settingsButton.onClick.AddListener(OnSettingsButton);
        _homeButton.onClick.AddListener(OnHomeButton);
    }

    private void OnSettingsButton()
    {
        if (_gameSettings == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_GameSettings>(out _gameSettings))
            {
                Debug.LogError("Null Exception : UI_GameSettings"); 
            }
        }
        _gameSettings.OpenUI(false);
    }

    private void OnHomeButton()
    {
       _soundManager.Play(Strings.Sounds.UI_BUTTON);

       _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.PausePanel.HOMEBUTTON
                , confirmCallback: GoHome
                , cancelCallback: null
                )
            );
    }

    private void GoHome()
    {
        if (!_sceneManagerEx.CurrentSceneType.Equals(Scenes.PracticeModeScene))
            _gameManager.ChangeState(Enums.PlayerDayCycleState.StartStore);
        else
            _gameManager.ChangeState(_playerData.DayCycleState);

        CloseUI();
        _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.HomeScene);
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
        OnUIOpen?.Invoke();
        Time.timeScale = 0;
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        Time.timeScale = 1;
        base.CloseUI(isSound, true);
        OnUIClose?.Invoke();
    }
}
