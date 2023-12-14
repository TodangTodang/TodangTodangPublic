using UnityEngine;

public class TutorialDayEndEventOperator : TutorialEventOperator
{
    [SerializeField] private UI_TutorialDay1End _uiTutorialDay1End;
    [SerializeField] private TutorialController _tutorialOpenMarket;
    [SerializeField] private TutorialController _tutorialUpgrade;
    [SerializeField] private TutorialController _tutorialDayEnd;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private DataManager _dataManager;
    private PlayerData _playerData;
    private UI_Inventory _uiInventory;

    private void Start()
    {
        Init();
        _tutorialOpenMarket.OnTutorialEnd += TurnToUpgradeTutorial;
        _tutorialUpgrade.OnTutorialEnd += TurnToDayEndTutorial;
        if (_gameManager.GetDayCycleState() == Enums.PlayerDayCycleState.OpenMarket)
        {
            ChangeTutorialState(Enums.Tutorial.DayEndState.OpenMarket);
        }
        else
        {
            if (!_playerData.IsCompleteUpgradeTutorial) 
                ChangeTutorialState(Enums.Tutorial.DayEndState.Upgrade);
            else
                ChangeTutorialState(Enums.Tutorial.DayEndState.DayEnd);
        }
    }

    private void Init()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
        _dataManager = DataManager.Instance;
        _playerData = _gameManager.GetPlayerData();
#if UNITY_EDITOR
        #region Check Null Exception
       
        DebugUtil.AssertNullException(_gameManager, "GameManager");
        DebugUtil.AssertNullException(_uiManager, "UIManager");
        DebugUtil.AssertNullException(_dataManager, "DataManager");
        DebugUtil.AssertNullException((_playerData != null), "PlayerData");

        DebugUtil.AssertNotAllocateInInspector(_uiTutorialDay1End, "_uiTutorialDay1End");
        DebugUtil.AssertNotAllocateInInspector(_tutorialOpenMarket, "_tutorialOpenMarket");
        DebugUtil.AssertNotAllocateInInspector(_tutorialUpgrade, "_tutorialUpgrade");
        DebugUtil.AssertNotAllocateInInspector(_tutorialDayEnd, "_tutorialDayEnd");

        #endregion
#endif
    }

    public override bool SetNextEvent(TutorialEventType eventType)
    {
        switch (eventType)
        {
            case TutorialEventType.UpdatePlayerMoney: 
                return UpdatePlayerMoney();
            default: 
                return false;
        }
    }

    private bool UpdatePlayerMoney()
    {
        if (_uiInventory == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_Inventory>(out _uiInventory))
            {
                Debug.LogError("Null Exception : UI_Inventory");
                return false;
            }
        }
        _playerData.UpdateMoney(Numbers.TUTORIAL_GIFT_MONEY);
        _uiInventory.UpdatePlayerMoneyUI(_playerData.Money);
        _uiTutorialDay1End.UpdateMoneyText(_playerData.Money);
        return true;
    }

    private void TurnToUpgradeTutorial()
    {
        ChangeTutorialState(Enums.Tutorial.DayEndState.Upgrade);
    }

    private void TurnToDayEndTutorial()
    {
        ChangeTutorialState(Enums.Tutorial.DayEndState.DayEnd);
        _playerData.UpdateCompleteUpgradeTutorial(true);
        _dataManager.SaveAllData();
    }

    private bool ChangeTutorialState(Enums.Tutorial.DayEndState dayEndState)
    {
        switch (dayEndState)
        {
            case Enums.Tutorial.DayEndState.OpenMarket:
                _tutorialOpenMarket.gameObject.SetActive(true);
                _tutorialUpgrade.gameObject.SetActive(false);
                _tutorialDayEnd.gameObject.SetActive(false);
                break;
            case Enums.Tutorial.DayEndState.Upgrade:
                _tutorialOpenMarket.gameObject.SetActive(false);
                _tutorialUpgrade.gameObject.SetActive(true);
                _tutorialDayEnd.gameObject.SetActive(false);
                break;
            case Enums.Tutorial.DayEndState.DayEnd:
                _tutorialOpenMarket.gameObject.SetActive(false);
                _tutorialUpgrade.gameObject.SetActive(false);
                _tutorialDayEnd.gameObject.SetActive(true);
                break;
        }
        return true;
    }
}
