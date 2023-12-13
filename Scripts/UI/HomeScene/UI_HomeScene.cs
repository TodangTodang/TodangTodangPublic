using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HomeScene : UI_Base
{
    #region SerializeField
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _decoStoreButton;
    [SerializeField] private Button _practiceModeButton;
    [SerializeField] private Button _startStoreButton;
    [SerializeField] private Button _openMarketButton;
    [SerializeField] private Button _dayEndButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _financialHelpButton;

    [SerializeField] private TextMeshProUGUI _dateText;
    [SerializeField] private TextMeshProUGUI _starLevelText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    #endregion

    private bool _isInitPlayerMoneyUI;

    private UI_Inventory _uiInventory;
    private UI_DecoStore _uiDecoStore;
    private UI_Market _uiMarket;
    private UI_GameSettings _uiGameSettings;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private DataManager _dataManager;
    private SceneManagerEx _sceneManagerEx;
    private SoundManager _soundManager;
    private PlayerData _playerData;
    private FinancialHelpController _financialHelpController;

    private void Start()
    {
        Init();
        InitButtons();
        _isInitPlayerMoneyUI = true;
        SetPlayerData();
        _playerData.PlayerDataChange += SetPlayerData;
        ActivateFinancialHelp();
    }

    private void Init()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_dataManager == null) _dataManager = DataManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        #region Null Exception
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : SceneManagerEx");
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
        #endregion

        if (_playerData == null) _playerData = _gameManager.GetPlayerData();
        if (_financialHelpController == null) _financialHelpController = new FinancialHelpController(_financialHelpButton);

        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
        Debug.Assert(_financialHelpController != null, "Null Exception : FinancialHelpController");
    }

    private void InitButtons()
    {
        #region Null Exception
        Debug.Assert(_inventoryButton != null, "Null Exception : _inventoryButton");
        Debug.Assert(_decoStoreButton != null, "Null Exception : _decoStoreButton");
        Debug.Assert(_practiceModeButton != null, "Null Exception : _practiceModeButton");
        Debug.Assert(_startStoreButton != null, "Null Exception : _startStoreButton");
        Debug.Assert(_openMarketButton != null, "Null Exception : _openMarketButton");
        Debug.Assert(_dayEndButton != null, "Null Exception : _dayEndButton");
        Debug.Assert(_settingButton != null, "Null Exception : _settingButton");
        Debug.Assert(_dateText != null, "Null Exception : _dateText");
        Debug.Assert(_starLevelText != null, "Null Exception : _starLevelText");
        Debug.Assert(_moneyText != null, "Null Exception : _moneyText");
        #endregion

        _inventoryButton.onClick.AddListener(OpenInventory);
        _decoStoreButton.onClick.AddListener(OpenDecoStore);
        _practiceModeButton.onClick.AddListener(LoadPracticeModeScene);
        _startStoreButton.onClick.AddListener(LoadGameScene);
        _openMarketButton.onClick.AddListener(OpenMarket);
        _dayEndButton.onClick.AddListener(DayEnd);
        _settingButton.onClick.AddListener(OpenSettings);
        _financialHelpButton.onClick.AddListener(GetFinancialHelp);
    }

    public void SetPlayerData()
    {
        _dateText.text = $"Day {_playerData.Date}";
        _starLevelText.text = $"{_playerData.Star * 0.1f}";
        if (_isInitPlayerMoneyUI)
        {
            _moneyText.text = _playerData.Money.ToString("N0");
            _isInitPlayerMoneyUI = false;
        } else
        {
            _moneyText.text = UpdatePlayerMoneyEffect();
        }
    }

    public void SetDayCycleButton(Enums.PlayerDayCycleState state)
    {
        switch (state)
        {
            case Enums.PlayerDayCycleState.StartStore:
                _startStoreButton.gameObject.SetActive(true);
                _openMarketButton.gameObject.SetActive(false);
                _dayEndButton.gameObject.SetActive(false);
                break;
            case Enums.PlayerDayCycleState.OpenMarket:
                _startStoreButton.gameObject.SetActive(false);
                _openMarketButton.gameObject.SetActive(true);
                _dayEndButton.gameObject.SetActive(false);
                break;
            case Enums.PlayerDayCycleState.DayEnd:
                _startStoreButton.gameObject.SetActive(false);
                _openMarketButton.gameObject.SetActive(false);
                _dayEndButton.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private string UpdatePlayerMoneyEffect()
    {
        string currentMoneyText = _moneyText.text.Replace(",", string.Empty);
        if (int.TryParse(currentMoneyText, out int currentMoney))
        {
            Color emphasizeColor = (currentMoney < _playerData.Money) ? Colors.MoonYellow : Colors.Pink;
            UIEffect.EmphasizeText(_moneyText, currentMoney, _playerData.Money, Colors.TextBlue, emphasizeColor);
        }

        return _playerData.Money.ToString("N0");
    }

    private void OpenInventory()
    {
        if (_uiManager.TryGetUIComponent<UI_Inventory>(out _uiInventory))
            _uiInventory.OpenUI();
    }

    private void OpenDecoStore()
    {
        if (_uiManager.TryGetUIComponent<UI_DecoStore>(out _uiDecoStore))
            _uiDecoStore.OpenUI();
    }

    private void LoadPracticeModeScene()
    {
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.HomeSceneMenu.LOAD_PRACTICE_MODE_POPUP,
                confirmCallback: () =>
                    {
                        _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.PracticeModeScene);
                    }
                )
            );
    }

    private void LoadGameScene()
    {
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.GameScene);
    }

    private void OpenMarket()
    {
        if (_uiManager.TryGetUIComponent<UI_Market>(out _uiMarket))
            _uiMarket.OpenUI();
    }

    private void ActivateFinancialHelp()
    {
        if (_playerData.isNeedHelp)
        {
            _financialHelpButton.gameObject.SetActive(true);
        }
        else
        {
            _financialHelpButton.gameObject.SetActive(false);
        }
    }

    private void GetFinancialHelp()
    {
        _financialHelpController.ShowRequestHelp();
    }

    public void DayEnd()
    {
        if (_uiManager.TryGetUIComponent<UI_ScreenFader>(out UI_ScreenFader fader))
            fader.StartFadeCircle();
        Invoke(nameof(DelayDayEnd), 2f);
    }

    private void DelayDayEnd()
    {
        _gameManager.DayEnd();
        _financialHelpButton.gameObject.SetActive(false);
        _playerData.UpdateNeedHelp(false);  
    }

    private void OpenSettings()
    {
        if (_uiManager.TryGetUIComponent<UI_GameSettings>(out _uiGameSettings))
             _uiGameSettings.OpenUI();
    }

    private void OnDestroy()
    {
        if (_dataManager != null)
        {
            _gameManager.GetPlayerData().PlayerDataChange -= SetPlayerData;
        }
    }
}
