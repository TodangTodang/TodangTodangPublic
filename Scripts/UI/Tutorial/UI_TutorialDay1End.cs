using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialDay1End : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _openMarketButton;
    [SerializeField] private Button _dayEndButton;
    [SerializeField] private Button _recipeTabButton;
    [SerializeField] private Button _kitchenTabButton;
    [SerializeField] private Button _sirutteokSlotButton;
    [SerializeField] private Button _steamerSlotButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _starLevelText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _upgradeButtonText;
    [SerializeField] private TutorialOnClick[] _openMarketOnClickListeners;
    [SerializeField] private TutorialOnClick[] _dayEndOnClickListeners;
    #endregion

    private int _clickIdx = 0;
    private bool _isRecipeUpgrade = false;
    private bool _isOpenMarketState = true;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private SoundManager _soundManager;
    private PlayerData _playerData;

    private UI_Inventory _uiInventory;
    private UI_Market _uiMarket;
    private UI_HomeScene _uiHomeScene;

    private void Awake()
    {
        InitSerializeFields();
    }

    private void Start()
    {
        InitManagers();
    }

    private void InitSerializeFields()
    {
        #region Check Null Exception
#if UNITY_EDITOR
        Debug.Assert(_inventoryButton, "Null Exception : _inventoryButton");
        Debug.Assert(_openMarketButton, "Null Exception : _openMarketButton");
        Debug.Assert(_dayEndButton, "Null Exception : _dayEndButton");
        Debug.Assert(_recipeTabButton, "Null Exception : _recipeTabButton");
        Debug.Assert(_kitchenTabButton, "Null Exception : _kitchenTabButton");
        Debug.Assert(_sirutteokSlotButton, "Null Exception : _sirutteokSlotButton");
        Debug.Assert(_steamerSlotButton, "Null Exception : _steamerSlotButton");
        Debug.Assert(_upgradeButton, "Null Exception : _upgradeButton");
        Debug.Assert(_closeButton, "Null Exception : _closeButton");
        Debug.Assert(_starLevelText, "Null Exception : _starLevelText");
        Debug.Assert(_moneyText, "Null Exception : _moneyText");
        Debug.Assert(_upgradeButtonText, "Null Exception : _upgradeButtonText");
        Debug.Assert(_openMarketOnClickListeners.Length != 0, "Null Exception : _openMarketOnClickListeners");
        Debug.Assert(_dayEndOnClickListeners.Length != 0, "Null Exception : _dayEndOnClickListeners");
#endif
        #endregion

        _inventoryButton.onClick.AddListener(OpenInventory);
        _openMarketButton.onClick.AddListener(OpenMarket);
        _dayEndButton.onClick.AddListener(DayEnd);
        _sirutteokSlotButton.onClick.AddListener(SelectSirutteokRecipe);
        _steamerSlotButton.onClick.AddListener(SelectSteamer);
        _upgradeButton.onClick.AddListener(UpgradeEvent);
        _closeButton.onClick.AddListener(CloseInventory);
    }

    private void InitManagers()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_playerData == null) _playerData = _gameManager.GetPlayerData();

        #region Check Null Exception
#if UNITY_EDITOR
        Debug.Assert(_gameManager, "Null Exception : GameManager");
        Debug.Assert(_uiManager, "Null Exception : UIManager");
        Debug.Assert(_soundManager, "Null Exception : SoundManager");
        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
#endif
        #endregion
    }

    public void UpdateMoneyText(int updatedMoney)
    {
        string curMoenyStr = _moneyText.text.Replace(",", String.Empty);
        if (int.TryParse(curMoenyStr, out int curMoney))
        {
            
            UIEffect.EmphasizeText(_moneyText, curMoney, updatedMoney, Colors.TextBlue, Colors.MoonYellow);
        }
        _moneyText.text = updatedMoney.ToString("N0");
    }

    private void OpenInventory()
    {
        if (_uiInventory == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_Inventory>(out _uiInventory))
            {
                Debug.LogError("Null Exception : UI_Inventory");
                return;
            }
        }
        _uiInventory.OpenUI();
        SetNextDayEndTutorialAction();
        SetPlayerData();
        _recipeTabButton.onClick.AddListener(OpenRecipeTab);
        _kitchenTabButton.onClick.AddListener(OpenKitchenTab);
    }

    private void OpenMarket()
    {
        if (_uiMarket == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_Market>(out _uiMarket))
            {
                Debug.LogError("Null Exception : UI_Market");
                return;
            }
        }
        _uiMarket = _uiManager.GetUIComponent<UI_Market>();
        _uiMarket.OpenUI();
        SetNextOpenMarketTutorialAction();
        _gameManager.ActivateEscapeInput(false);
        _uiMarket.OnTutorialClosed += SetNextOpenMarketTutorialAction;
    }

    private void DayEnd()
    {
        if (_uiHomeScene == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_HomeScene>(out _uiHomeScene))
            {
                Debug.LogError("Null Exception : UI_HomeScene");
                return;
            }
        }
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        _uiHomeScene.DayEnd();
        PlayerPrefs.SetInt(Strings.Prefs.IS_FIRST_TUTORIAL, 0);
        Destroy(transform.parent.gameObject);
    }

    private void OpenRecipeTab()
    {
        _uiInventory.GetTabList()[(int)Enums.InventoryType.Recipe].CallOnTabClicked();
        SetNextDayEndTutorialAction();
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }

    private void OpenKitchenTab()
    {
        _uiInventory.GetTabList()[(int)Enums.InventoryType.Kitchen].CallOnTabClicked();
        SetNextDayEndTutorialAction();
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }

    private void SetPlayerData()
    {
        _starLevelText.text = $"{_playerData.Star * 0.1f}";
        _moneyText.text = _playerData.Money.ToString("N0");
    }

    private void SelectSirutteokRecipe()
    {
        _uiInventory.GetSlotList()[1].CallOnClicked();
        SetNextDayEndTutorialAction();
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }

    private void SelectSteamer()
    {
        _uiInventory.GetSlotList()[0].CallOnClicked();
        SetNextDayEndTutorialAction();
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }

    private void UpgradeEvent()
    {
        _uiInventory.GetUIInventoryDetail().CallOnSellUpgradeBtnClicked();
        if (!_isRecipeUpgrade)
        {
            _upgradeButtonText.text = "업그레이드";
            _isRecipeUpgrade = true;
        }
        SetNextDayEndTutorialAction();
    }

    private void CloseInventory()
    {
        _uiInventory.CloseUI();
        SetNextDayEndTutorialAction();
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }

    private void SetNextOpenMarketTutorialAction(bool isMarketClose = false)
    {
        if (_clickIdx < _openMarketOnClickListeners.Length)
        {
            _openMarketOnClickListeners[_clickIdx++].IsClicked = true;
        }
        if (isMarketClose) _gameManager.ActivateEscapeInput(true);
    }

    private void SetNextDayEndTutorialAction()
    {
        if (_isOpenMarketState)
        {
            _isOpenMarketState = false;
            _clickIdx = 0;
        }
        if (_clickIdx < _dayEndOnClickListeners.Length)
        {
            _dayEndOnClickListeners[_clickIdx++].IsClicked = true;
        }
    }
}
