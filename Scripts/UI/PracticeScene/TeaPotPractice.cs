using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaPotPractice : TeaPot
{
    protected override void Initialize()
    {
        GameManager manager = GameManager.Instance;
        PlayerData playerData = manager.GetPlayerData();

        List<KitchenUtensilInfoData> utensilList = playerData.GetInventory<KitchenUtensilInfoData>();
        data = utensilList.Find(x => x.DefaultData.name == base.name);

        _interactionUI = UIManager.Instance.GetUIComponent<UI_IngredientBoxPractice>();
        _interactionUI.gameObject.SetActive(false);

        CanInteractWithPlayer = true;
        IsPlaceable = true;

        _coolTime = (float)data.DefaultData.SpeedUpgradeInfo[data.Level - 1];    // TODO: to reflect level
        _currentTime = _coolTime;
        _isWorking = false;
        progressBar.SetActive(false);

        interactionPos = foodPos[0];
        successSound = Strings.Sounds.KITCHEN_BASIC_SUCCESS;
    }
}
