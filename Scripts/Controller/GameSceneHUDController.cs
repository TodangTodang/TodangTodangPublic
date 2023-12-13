using Unity.Profiling;

public class GameSceneHUDController
{
    private UI_GameSceneHUD _gameSceneHud;
    private GameOperator _operator;
    
    public void Init(UI_GameSceneHUD gameSceneHud)
    {
        _gameSceneHud = gameSceneHud;
    }

    public void SetDay(int day)
    {
        _gameSceneHud.SetDay(day);
    }


    public void ChangeTime(float timeRate)
    {
        _gameSceneHud.SetTime(timeRate);
    }
    
    public void SetLeftCustomerCount(int customerCount, bool isInit = false)
    {
        _gameSceneHud.SetLeftCustomerCount(customerCount, isInit);
    }

    public void SetEarnMoney(int money)
    {
        _gameSceneHud.SetEarnMoneyText(money);
    }

    public void FlashSpendDay(float interval)
    {
        _gameSceneHud.FlashSpendDayEffect(interval);

    }
}
