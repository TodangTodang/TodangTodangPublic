using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CookBook : UI_Base
{
    [Header("Basic Frame")]
    [SerializeField] private Transform _menuSlotArea;
    [SerializeField] private GameObject _teaRecipes;
    [SerializeField] private GameObject _ricecakeRecipes;
    [SerializeField] private Transform _cookStepArea;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private Sprite _teaIcon;
    private CookBookSO _teaCookBook;

    private PlayerData _playerData;
    private List<ResultInfoSO> _unlockedRecipes;
    private List<CookBookSO> _recipes;

    private UI_CookStepUpdater _cookStepUpdater;
    private List<UI_RecipeSlot> _slots;
    private UI_RecipeSlot _currentSlot;

    private bool _teaAvailable;

    public event Action OnUIOpen;
    public event Action OnUIClose;

    public Action CheckNewMark;

    private GameManager _gameManager;
    private DataManager _dataManager;
    private ResourceManager _resourceManager;

    public void Init()
    {
        _gameManager = GameManager.Instance;
        _dataManager = DataManager.Instance;
        _resourceManager = ResourceManager.Instance;
        
        Debug.Assert(_gameManager, $"_gameManager {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_dataManager, $"_dataManager {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_resourceManager, $"_resourceManager {Strings.DebugLog.INIT_PROBLEM}");
            
        _cookStepArea.TryGetComponent<UI_CookStepUpdater>(out _cookStepUpdater);
        _closeBtn.onClick.AddListener(ClosePanel);
        _playerData =_gameManager.GetPlayerData();
        Debug.Assert(_playerData != null, $"_playerData {Strings.DebugLog.INIT_PROBLEM}");
        
        _unlockedRecipes = _playerData.GetUnlockedRecipe();
        _recipes = GetAvailableRecipes();
        CreateSlots();
    }

    private List<CookBookSO> GetAvailableRecipes()
    {
        CookBookSO[] allRecipes = DataManager.Instance.GetDefaultDataArray<CookBookSO>();
        Array.Sort(allRecipes, (a, b) => a.ID.CompareTo(b.ID));


        List<RecipeInfoData> playerRecipes = _playerData.GetInventory<RecipeInfoData>();

        List<CookBookSO> availableRecipes =  new List<CookBookSO>();

        foreach (RecipeInfoData recipe in playerRecipes)
        {
            if (recipe.Level > 0)
            {
                if (recipe.DefaultData.Type == Enums.FoodType.Ricecake)
                {
                    availableRecipes.Add(allRecipes[recipe.DefaultData.ID]);
                }
                else
                {
                    _teaAvailable = true;
                }
            }
        }

        availableRecipes.Sort((a, b) => a.ID.CompareTo(b.ID));

        _teaCookBook = Array.Find<CookBookSO>(allRecipes, recipe => recipe.ID >= 100);

        return availableRecipes;
    }


    private void CreateSlots()
    {
        _slots = new List<UI_RecipeSlot>();

        bool firstSlotchecked = false;
        foreach (var recipe in _recipes)
        {
            GameObject slot = _resourceManager.Instantiate(Strings.Prefabs.UI_RECIPE_SLOT, _menuSlotArea);
            slot.transform.localScale = Vector3.one;
            bool _isNew = _unlockedRecipes.Contains(recipe.Result as ResultInfoSO);
            UI_RecipeSlot uiRecipeSlot;
            if (slot.TryGetComponent<UI_RecipeSlot>(out uiRecipeSlot))
            {
                uiRecipeSlot.Init(recipe, ChangeSlot, CheckNewMark, _isNew);
                _slots.Add(uiRecipeSlot);
                if (!firstSlotchecked)
                {
                    _currentSlot = uiRecipeSlot;
                    _currentSlot.SelectRecipe(true);
                    _cookStepUpdater.UpdateCookStep(_currentSlot.recipe);

                    firstSlotchecked = true;
                }
            }
            else
            {
                Debug.LogWarning("slot Object에 UI_RecipeSlot Component가 없습니다");
            }
        }
        
        if (_teaAvailable)
        {
            GameObject slot = _resourceManager.Instantiate(Strings.Prefabs.UI_RECIPE_SLOT, _menuSlotArea);
            slot.transform.localScale = Vector3.one;


            bool _isNew = false;
            foreach (var tea in _unlockedRecipes)
            {
                if (tea.Type == Enums.FoodType.Tea)
                {
                    _isNew = true;
                    break;
                }
            }

            UI_RecipeSlot uiRecipeSlot;
            if (slot.TryGetComponent<UI_RecipeSlot>(out uiRecipeSlot))
            {
                uiRecipeSlot.Init(_teaCookBook,_teaIcon, ChangeSlot, CheckNewMark, _isNew);
            }
        }
    }


    public void ChangeSlot(UI_RecipeSlot slot)
    {
        _currentSlot.SelectRecipe(false);
        _currentSlot = slot;
        _currentSlot.SelectRecipe(true);

        if (_currentSlot.recipe == _teaCookBook)
        {
            _ricecakeRecipes.SetActive(false);
            _teaRecipes.SetActive(true);
        }
        else
        {
            _ricecakeRecipes.SetActive(true);
            _teaRecipes.SetActive(false);
            _cookStepUpdater.UpdateCookStep(_currentSlot.recipe);
        }
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
        if (_slots != null) ChangeSlot(_slots[0]);
        OnUIOpen?.Invoke();
        Time.timeScale = 0f;
    }

    private void ClosePanel()
    {
        CloseUI();
    }
    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        Time.timeScale = 1f;
        base.CloseUI(isSound, isAnimated);
        OnUIClose?.Invoke();
    }

}
