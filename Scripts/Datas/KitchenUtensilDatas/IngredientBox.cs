using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : KitchenInteraction
{
    protected UI_IngredientBox _interactionUI;

    [SerializeField] private List<GameObject> _storageIngredients;
    [SerializeField] private GameObject _waiting;

    protected override void Initialize()
    {
        _interactionUI = UIManager.Instance.GetUIComponent<UI_IngredientBox>();
        _interactionUI.gameObject.SetActive(false);

        CanInteractWithPlayer = true;
    }

    public override void Interaction()
    {
        if (player.Ingredient != null) return;

        soundManager.Play(Strings.Sounds.KITCHEN_INGREDIENTBOX);

        _interactionUI.Init(Enums.FoodType.Ricecake);
        _interactionUI.OnSelection += OnIngredientSelection;
        _interactionUI.OnCancel += OnCancelSelection;
        _interactionUI.OpenUI(false);

        player.Ingredient = _waiting;
    }


    private void OnIngredientSelection(IngredientInfoSO ingredient)
    {
        if (ingredient.Prefab == null)
        {
            player.Ingredient = null;
            player.ResetState();
            return;
        }

        GameObject obj = ResourceManager.Instance.Instantiate($"Foods/{ingredient.Prefab.name}");
        SetObejctsParent(obj, player.foodPos);
        player.Ingredient = obj;
    }

    private void OnCancelSelection()
    {
        soundManager.Play(Strings.Sounds.KITCHEN_INGREDIENTBOX);
        player.ResetState();
        player.Ingredient = null;
    }
}
