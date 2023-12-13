using UnityEngine;
using UnityEngine.UI;

public class UI_PracticeModeScene : UI_Base
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _recipeButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private GameObject _newRecipeMark;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private SceneManagerEx _sceneManagerEx;

    private UI_CookBook _uiCookBook;
    private UI_PausePanel _uiPausePanel;

    private Player _player;
    private PlayerData _playerData;

    private void Awake()
    {
        InitButtons();
    }

    private void Start()
    {
        Init();
        SetCookBook();
    }

    private void Init()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;

        #region Null Exception
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : SceneManagerEx");
        #endregion

        if (_playerData == null) _playerData = _gameManager.GetPlayerData();
        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
    }

    private void InitButtons()
    {
        #region Null Exception
        Debug.Assert(_exitButton != null, "Null Exception : _exitButton");
        Debug.Assert(_recipeButton != null, "Null Exception : _recipeButton");
        Debug.Assert(_settingButton != null, "Null Exception : _settingButton");
        #endregion

        _exitButton.onClick.AddListener(ExitPracticeMode);
        _recipeButton.onClick.AddListener(OpenRecipeBook);
        _settingButton.onClick.AddListener(OpenPausePanel);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        Debug.Assert(_player != null, "Null Exception : Player");

        if (_uiManager == null) _uiManager = UIManager.Instance;
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");

        UI_PracticeHelp practiceHelp = _uiManager.GetUIComponent<UI_PracticeHelp>();
        practiceHelp.SetPlayerInput(_player.Input);
        practiceHelp.gameObject.SetActive(false);
    }

    private void ExitPracticeMode()
    {
        _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.PracticeModeScene.EXIT_POPUP_CONTENT,
                confirmCallback: LoadHomeScene
                )
            );
    }

    private void OpenRecipeBook()
    {
        if (_uiCookBook == null) _uiManager.TryGetUIComponent<UI_CookBook>(out _uiCookBook);
        if (_uiCookBook != null) _uiCookBook.OpenUI();
    }

    private void OpenPausePanel()
    {
        if (_uiPausePanel == null) _uiManager.TryGetUIComponent<UI_PausePanel>(out _uiPausePanel);
        if (_uiPausePanel != null) _uiPausePanel.OpenUI();
    }

    private void SetCookBook()
    {
        if (_uiCookBook == null) _uiManager.TryGetUIComponent<UI_CookBook>(out _uiCookBook);
        if (_uiCookBook != null)
        {
            _uiCookBook.CheckNewMark = UpdateNewMark;
            _uiCookBook.Init();
            UpdateNewMark();
        }
    }

    private void UpdateNewMark()
    {
        bool _newRecipeExists = _playerData.GetUnlockedRecipe().Count > 0;
        _newRecipeMark.SetActive(_newRecipeExists);
    }


    private void LoadHomeScene()
    {
        _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.HomeScene);
    }
}
