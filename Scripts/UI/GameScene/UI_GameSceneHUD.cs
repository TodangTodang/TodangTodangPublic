using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSceneHUD : UI_Base
{
    [SerializeField] private Slider _spendDay;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private TMP_Text _earnMoneyText;
    [SerializeField] private TMP_Text _leftCustomerCount;

    [SerializeField] private Button _earlyExitButton;
    [SerializeField] private Button _cookBookButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _newRecipeMark;

    private Color _normalColor;
    private Color _changingColor;
    private Color _spendDayColor;

    private GameManager _gameManager;
    private UIManager _uiManager;

    private UI_CookBook _uiCookBook;
    private UI_PausePanel _uiPausePanel;


    private void Awake()
    {
        #region Check Null SerializeFields
        Debug.Assert(_spendDay != null, "Null Exception : _spendDay");
        Debug.Assert(_dayText != null, "Null Exception : _dayText");
        Debug.Assert(_earnMoneyText != null, "Null Exception : _earnMoneyText");
        Debug.Assert(_leftCustomerCount != null, "Null Exception : _leftCustomerCount");
        Debug.Assert(_earlyExitButton != null, "Null Exception : _earlyExitButton");
        Debug.Assert(_cookBookButton != null, "Null Exception : _recipeBookButton");
        Debug.Assert(_pauseButton != null, "Null Exception : _settingButton");
        Debug.Assert(_newRecipeMark != null, "Null Exception : _newRecipeMark");
        #endregion
    }

    public void Init(Action earlyExitAction)
    {
        InitManagers();
        InitButtons(earlyExitAction);

        ColorUtility.TryParseHtmlString("#DCEFFF", out _normalColor);
        ColorUtility.TryParseHtmlString("#FF9886", out _changingColor);
        ColorUtility.TryParseHtmlString("#FF9D9D", out _spendDayColor);

        if (!_uiManager.TryGetUIComponent(out UI_CookBook cookbook))
        {
            Debug.LogError("Null Exception : UI_CookBook");
            return;
        }

        cookbook.CheckNewMark = UpdateNewMark;

        UpdateNewMark();
        cookbook.Init();
        cookbook.gameObject.SetActive(false);
    }

    private void InitManagers()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;

        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
    }

    private void InitButtons(Action earlyExitAction)
    {
        _earlyExitButton.onClick.AddListener(() =>
        {
            Debug.Assert(earlyExitAction != null, "Null Exception : earlyExitAction");
            CallOnEarlyExitEvent(earlyExitAction);
        });
        _cookBookButton.onClick.AddListener(OpenCookBook);
        _pauseButton.onClick.AddListener(OpenPausePanel);
    }

    public void SetTime(float rate)
    {
        _spendDay.value = rate;
    }

    public void FlashSpendDayEffect(float interval)
    {
        Image background = _spendDay.GetComponentInChildren<Image>();
        background.DOColor(_spendDayColor, interval).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetLeftCustomerCount(int customerCount, bool isInit = false)
    {
        if (isInit)
        {
            _leftCustomerCount.text = customerCount.ToString();
        }
        else
        {
            int currentCount = int.Parse(_leftCustomerCount.text);
            UIEffect.EmphasizeText(_leftCustomerCount, currentCount, customerCount, _normalColor, _changingColor);
        }
    }

    public void SetDay(int day)
    {
        _dayText.text = day.ToString();
    }

    public void SetEarnMoneyText(int money)
    {
        string moneyText = _earnMoneyText.text.Replace(",", "");
        int currentMoney = int.Parse(moneyText);
        if (money > currentMoney)
        {
            UIEffect.EmphasizeText(_earnMoneyText, currentMoney, money, Colors.TextBlue, Colors.MoonYellow);
        }
    }

    private void UpdateNewMark()
    {
        bool _newRecipeExists = _gameManager.GetPlayerData().GetUnlockedRecipe().Count > 0;
        _newRecipeMark.SetActive(_newRecipeExists);
    }

    private void CallOnEarlyExitEvent(Action earlyExitAction)
    {
        _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.GameSceneMenu.EARLY_EXIT_WARNING,
                confirmCallback: () =>
                {
                    earlyExitAction?.Invoke();
                    Time.timeScale = 0.0f;
                }
                )
            );
    }
    
    private void OpenCookBook()
    {
        if (_uiCookBook == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_CookBook>(out _uiCookBook))
            {
                Debug.LogError("Null Exception : UI_CookBook");
                return;
            }
        }
        _uiCookBook.OpenUI();
    }   

    private void OpenPausePanel()
    {
        if (_uiPausePanel == null)
        {
            if (!_uiManager.TryGetUIComponent<UI_PausePanel>(out _uiPausePanel))
            {
                Debug.LogError("Null Exception : UI_PausePanel");
                return;
            }

        }
        _uiPausePanel.OpenUI();
    }

    public void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}
