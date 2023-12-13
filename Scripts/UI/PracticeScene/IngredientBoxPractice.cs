using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBoxPractice : IngredientBox
{
    protected override void Initialize()
    {
        _interactionUI = UIManager.Instance.GetUIComponent<UI_IngredientBoxPractice>();
        _interactionUI.gameObject.SetActive(false);

        CanInteractWithPlayer = true;
    }
}
