using UnityEngine;

public class HomeScene : BaseScene
{
    //public bool isPCVersion;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private ResourceManager _resourceManager;
    private SoundManager _soundManager;
    private PoolManager _poolManager;

    private UI_HomeScene _uiHomeScene;

    protected override bool Init()
    {
        if (!base.Init()) return false;
       
        
        #region Null Exception
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_resourceManager == null) _resourceManager = ResourceManager.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_poolManager == null) _poolManager = PoolManager.Instance;
        
        #endregion
        NewsSystem newsSystem = _gameManager.GetNewsSystem();
        Debug.Assert(newsSystem!= null,$"newsSystem {Strings.DebugLog.INIT_PROBLEM}");
        
        SceneType = Scenes.HomeScene;
        _uiHomeScene = _uiManager.GetUIComponent<UI_HomeScene>();
        _uiHomeScene.SetDayCycleButton(_gameManager.GetDayCycleState());

        // Tutorial
        PlayerData playerData = _gameManager.GetPlayerData();
        Enums.PlayerDayCycleState dayCycleState = _gameManager.GetDayCycleState();
        if (playerData.Date == 1 && dayCycleState == Enums.PlayerDayCycleState.StartStore)
        {
            _resourceManager.Instantiate(Strings.Prefabs.TUTORIAL_DAY1_START);
            if (!PlayerPrefs.HasKey(Strings.Prefs.IS_FIRST_TUTORIAL))
                PlayerPrefs.SetInt(Strings.Prefs.IS_FIRST_TUTORIAL, 1);
            PlayerPrefs.SetInt(Strings.Prefs.IS_TUTORIAL_SKIP, 0);
        }
        else if (playerData.Date == 1 
            && (dayCycleState == Enums.PlayerDayCycleState.OpenMarket
            || dayCycleState == Enums.PlayerDayCycleState.DayEnd))
        {
            if (!PlayerPrefs.HasKey(Strings.Prefs.IS_TUTORIAL_SKIP)
                || PlayerPrefs.GetInt(Strings.Prefs.IS_TUTORIAL_SKIP) == 0)
                _resourceManager.Instantiate(Strings.Prefabs.TUTORIAL_DAY1_END);
        }

        Enums.PlayerEndingState state = playerData.EndingState;
        switch (state)
        {
            case Enums.PlayerEndingState.ContinuePlaying:
                break;
            case Enums.PlayerEndingState.BankruptcyEnding:
                _resourceManager.Instantiate("UI/UI_EndBankruptcyDialog");
                playerData.UpdateEndingState(Enums.PlayerEndingState.ContinuePlaying);
                break;
            case Enums.PlayerEndingState.GameOverEnding:
            case Enums.PlayerEndingState.GameClearEnding:
                playerData.UpdateEndingState(Enums.PlayerEndingState.ContinuePlaying);
                break;
            default:
                break;
        }
        newsSystem.CheckAchieveMent(playerData.Star);
        newsSystem.ViewNews();

        _soundManager.Play(Strings.Sounds.BGM_HOMESCENE, Enums.AudioType.Bgm);
        return true;
    }

    public override void Clear()
    {
        _poolManager.Clear();
        base.Clear();
    }

    public void ChangeDayCycleState(Enums.PlayerDayCycleState state)
    {
        _uiHomeScene.SetDayCycleButton(state);
    }
}
