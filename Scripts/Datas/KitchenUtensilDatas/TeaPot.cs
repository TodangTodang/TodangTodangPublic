using System.Collections.Generic;
using UnityEngine;

public class TeaPot : KitchenInteraction
{
    protected UI_IngredientBox _interactionUI;

    [SerializeField] private List<GameObject> _storageIngredients;
    protected float _coolTime;
    protected float _currentTime;
    protected bool _isWorking;

    private void Update()
    {
        if (_isWorking)
        {
            _currentTime -= Time.deltaTime;
            progressFill.fillAmount = (float)(_coolTime - _currentTime) / _coolTime;
            if (_currentTime < 0)
            {
                _currentTime = _coolTime;
                CheckValidity();
                _isWorking = false;
                progressBar.SetActive(false);
            }
        }
    }

    protected override void Initialize()
    {
        GetUtensilData();
        _interactionUI = UIManager.Instance.GetUIComponent<UI_IngredientBox>();
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

    public override void Interaction()
    {
        if (player.Ingredient != null) return;
        if (_isWorking) return;
        if (ingredients.Count >= foodPos.Length) return;

        soundManager.Play(Strings.Sounds.KITCHEN_INGREDIENTBOX);

        _interactionUI.Init(Enums.FoodType.Tea);
        _interactionUI.OnSelection += OnIngredientSelection;
        _interactionUI.OnCancel += OnCancelSelection;
        _interactionUI.OpenUI(false);
    }

    private void OnIngredientSelection(IngredientInfoSO ingredient)
    {
        if (ingredient.Prefab == null) return;

        GameObject obj = ResourceManager.Instance.Instantiate($"Foods/{ingredient.Prefab.name}");
        obj.transform.position = foodPos[0].position;
        ingredients.Add(obj);

        _isWorking = true;
        progressBar.SetActive(true);
        soundManager.Play(Strings.Sounds.KITCHEN_WATER);
    }

    private void OnCancelSelection()
    {
        soundManager.Play(Strings.Sounds.KITCHEN_INGREDIENTBOX);

        player.ResetState();
        player.Ingredient = null;
    }

    public override void PickUp()
    {
        if (_isWorking) return;
        base.PickUp();
    }

    public override void PutDown()
    {
    }

}
