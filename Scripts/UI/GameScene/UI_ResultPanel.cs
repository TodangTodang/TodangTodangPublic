using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class UI_ResultPanel : UI_Base
{
    [SerializeField] private TMP_Text[] _customerText;
    [SerializeField] private Transform _disposeFoodPanel;
    [SerializeField] private TMP_Text _earnMoney; // 매출
    [SerializeField] private TMP_Text _netIncome; // 순이익
    [SerializeField] private TMP_Text _previousStarText;
    [SerializeField] private TMP_Text _currentStarText;
    [SerializeField] private TMP_Text _disposalCostsText;

    [SerializeField] private Image _backgroundPanel;

    [SerializeField] private Button _closeButton;

    private GameManager _gameManager;
    private ResourceManager _resourceManager;
    private PlayerData _playerData;

    public event Action OnUIOpen;
    public event Action<Enums.PlayerEndingState> OnSelectScene;


    private void Awake()
    {
        #region Null Exception
        Debug.Assert(_customerText != null, "Null Exception : _customerText");
        Debug.Assert(_disposeFoodPanel != null, "Null Exception : _disposeFoodPanel");
        Debug.Assert(_earnMoney != null, "Null Exception : _earnMoney");
        Debug.Assert(_netIncome != null, "Null Exception : _netIncome");
        Debug.Assert(_previousStarText != null, "Null Exception : _previousStarText");
        Debug.Assert(_currentStarText != null, "Null Exception : _currentStarText");
        Debug.Assert(_disposalCostsText != null, "Null Exception : _disposalCostsText");
        Debug.Assert(_backgroundPanel != null, "Null Exception : _backgroundPanel");
        Debug.Assert(_disposalCostsText != null, "Null Exception : _disposalCostsText");
        Debug.Assert(_closeButton != null, "Null Exception : _toHomeSceneButton");
        #endregion

        _closeButton.onClick.AddListener(() => CloseUI(false));
    }

    public void FadeBackground()
    {
        Time.timeScale = 0f;
        StartCoroutine(FadeBackground(0, 0.3922f, 1.3f));
    }

    private IEnumerator FadeBackground(float start, float targetAlpha, float duration)
    {
        yield return StartCoroutine(FadeOperator.FadeLinear(start, targetAlpha, duration, alpha =>
             {
                 _backgroundPanel.color = new Color(_backgroundPanel.color.r, _backgroundPanel.color.g, _backgroundPanel.color.b, alpha);
             }));
    }

    public void Init(ref SalesData data, int previousStar, int currentStar, int netIncome)
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_resourceManager == null) _resourceManager = ResourceManager.Instance;
        if (_playerData == null) _playerData = _gameManager.GetPlayerData();


        // 판매한 떡, 폐기된 재료 
        MakeDisposeFoodSlot(data.ExpiredFoods);

        // 응대한 손님
        int satisfiedCustomer = data.PerfectCustomerCount + data.GreatCustomerCount + data.SoSoCustomerCount;
        data.AngryCustomerCount += data.TotalCustomerCount - satisfiedCustomer - data.AngryCustomerCount;
        _customerText[0].text = data.TotalCustomerCount.ToString();
        _customerText[1].text = satisfiedCustomer.ToString();
        _customerText[2].text = data.AngryCustomerCount.ToString();

        // 폐기 비용 정리 
        _disposalCostsText.text = $"{data.TotalDisposeCount} x {Numbers.FIXED_DISPOALCOST} = {data.TotalDisposeCount * Numbers.FIXED_DISPOALCOST}";

        // 판매 금액 정리 
        _earnMoney.text = $"{data.EarnMoney}";
        _netIncome.text = $"{netIncome}";

        //평점 
        float pStar = Mathf.Clamp(previousStar * .1f, 0, 5f);
        float cStar = Mathf.Clamp(currentStar * .1f, 0, 5f);

        _previousStarText.text = $"{pStar}";
        _currentStarText.text = $"{cStar}";
        OnUIOpen?.Invoke();
    }

    private void MakeDisposeFoodSlot(Dictionary<IngredientInfoSO, int> foods)
    {
        foreach (var food in foods)
        {
            GameObject prefabs = ResourceManager.Instance.Instantiate("UI/UI_DisposeSlot");
            var slot = prefabs.GetComponent<UI_DisposeSlot>();
            slot.transform.SetParent(_disposeFoodPanel);
            slot.Init(food.Key.IconSprite, food.Value.ToString());
        }
    }

    private void LoadScene()
    {
        OnSelectScene.Invoke(_playerData.EndingState);
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        if (_playerData.EndingState.Equals(Enums.PlayerEndingState.BankruptcyEnding))
        {
            UI_BankruptcyDialog dialog;
            if (_resourceManager.Instantiate("UI/UI_StartBankruptcyDialog").TryGetComponent(out dialog))
            {
                dialog.FinishedDialogs += LoadScene;
            }
        }
        else
            LoadScene(); 

        base.CloseUI(false);
    }
}
