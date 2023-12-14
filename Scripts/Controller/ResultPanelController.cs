using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ResultPanelController : MonoBehaviour
{
    private SalesData _salesData;

    private DataManager _dataManager;
    private GameManager _gameManager;
    private PlayerData _playerData;

    [SerializeField] private UI_ResultPanel _resultPanel;

    private int _previousStar; 
    private int _currentStar;
    private int _netIncome; 

    public PlayerEndingState endingState = PlayerEndingState.ContinuePlaying;

    public void Init(ref SalesData data)
    {
        if (_dataManager == null) _dataManager = DataManager.Instance;
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_playerData == null) _playerData = _gameManager.GetPlayerData();

        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
        Debug.Assert(_resultPanel != null, "Null Exception : _resultPanel");

        _salesData = data;

        _resultPanel.OnSelectScene += HandleGameStatus;

        UpdatePlayData();
        UpdatePlayerStar();

        _resultPanel.Init(ref _salesData, _previousStar, _currentStar, _netIncome);

        CheckGameStatus();

        _gameManager.ChangeState(PlayerDayCycleState.OpenMarket);
        _playerData.UpdateEndingState(endingState);

        _dataManager.SaveAllData();
    }

    private void UpdatePlayerStar()
    {
        _previousStar = _playerData.Star;
        CalculateRatingRatio();
        _currentStar = _playerData.Star;
    }

    private void UpdatePlayData()
    {
        List<IngredientInfoData> expiredList = _playerData.RemoveExpiredItem();

        int totoalDisposeCount = 0;
        int totalDisposeCost; 

        if (expiredList != null)
        {
            foreach (IngredientInfoData expiredFood in expiredList)
            {
                _salesData.ExpiredFoods.Add(expiredFood.DefaultData, expiredFood.Quantity);
                totoalDisposeCount += expiredFood.Quantity;
                Debug.Log($"{expiredFood.DefaultData.Name},{expiredFood.Quantity}");
            }
        }

        _salesData.TotalDisposeCount = totoalDisposeCount;
        totalDisposeCost = totoalDisposeCount * Numbers.FIXED_DISPOALCOST; 
         _netIncome = _salesData.EarnMoney - totalDisposeCost;
        _playerData.UpdateMoney(_netIncome);

        CheckNeedHelp();
        _salesData.RestMoney = _playerData.Money;
    }

    private void CheckNeedHelp()
    {
        _playerData.UpdateNeedHelp(_playerData.Money < 500);
    }

    private void CalculateRatingRatio()
    {
        int star = 0;
        int totalCustomers = _salesData.TotalCustomerCount;
        int satisfiedCustomers = _salesData.PerfectCustomerCount + _salesData.GreatCustomerCount + _salesData.SoSoCustomerCount;

        int satisfactionRaio = (int)((double)satisfiedCustomers / totalCustomers * 100);

        if (satisfactionRaio >= 95) star += 3;
        else if (satisfactionRaio >= 80) star += 2;
        else if (satisfactionRaio >=65) star += 1;
        else if (satisfactionRaio >= 50) star += 0;
        else if (satisfactionRaio >= 35) star -= 1;
        else if (satisfactionRaio >= 20) star -= 2;
        else star -= 3;

        _playerData.UpdateStar(star);
    }

    private void CheckGameStatus()
    {
        if (_playerData.Star <= 0)
        {
            endingState = PlayerEndingState.GameOverEnding;
            _dataManager.DeletePlaySaveDataAll();
        }
        else if (_playerData.Money <= 0 && CheckInventoryIngredientDatas())
        {
            endingState = PlayerEndingState.BankruptcyEnding;
            _playerData.OnBankruptcy();
        }
        else if (_playerData.Star >= 50)
        {
            endingState = PlayerEndingState.GameClearEnding;
        }
        else
            endingState = PlayerEndingState.ContinuePlaying;
    }

    private void HandleGameStatus(PlayerEndingState endingState)
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        switch (endingState)
        {
            case PlayerEndingState.ContinuePlaying:
                SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.HomeScene);
                break;
            case PlayerEndingState.BankruptcyEnding:
                //AnalyticsManager.Ending("Bankrupt");
                SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.BankruptcyScene);
                Debug.Log("파산");
                break;
            case PlayerEndingState.GameOverEnding:
                //AnalyticsManager.Ending("GameOver");
                SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.GameOverScene);
                Debug.Log("게임 오버");
                break;
            case PlayerEndingState.GameClearEnding:
                //AnalyticsManager.Ending("GameClear");
                SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.GameClearScene);
                Debug.Log("게임 클리어");
                break;
        }
    }

    public bool CheckInventoryIngredientDatas()
    {
        List<IngredientInfoData> ingredientDatas = _playerData.GetInventory<IngredientInfoData>(); 

        foreach(var ingredientData in ingredientDatas)
        {
            if (ingredientData.Quantity > 0)
                return false; 
        }
        return true; 
    }
}
