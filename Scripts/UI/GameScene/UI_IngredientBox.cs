using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngredientBox : UI_Base
{
    [SerializeField] private Transform _slotContainer;
    [SerializeField] private Button _selectBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private GameObject _messages;

    protected GameManager _gameManager;
    protected PlayerData _playerData;
    protected List<IngredientInfoData> _ingredientDatas;
    protected List<IngredientInfoData> _ricecakeIngredients;
    protected List<IngredientInfoData> _teaIngredients;

    protected UI_IngredientBoxSlot currentSlot;
    private List<GameObject> _slots;

    public event Action OnUIOpen;
    public event Action OnUIClose;
    public Action<IngredientInfoSO> OnSelection;
    public event Action OnCancel;

    private int _defaultContainerHeight = 370;
    private int _maxVisibleContents = 4;
    private int _slotHeight = 200;

    private void Awake()
    {
        ResetData();
    }

    public override void OpenUI(bool isSound = false, bool isAnimated = true)
    {
        OnUIOpen?.Invoke();
        base.OpenUI(isSound, isAnimated);
    }

    public void Init(Enums.FoodType type)
    {
        _selectBtn.interactable = false;

        if (_slots == null) _slots = new List<GameObject>();

        List<IngredientInfoData> ingredients = type == Enums.FoodType.Ricecake? _ricecakeIngredients : _teaIngredients;

        SetContentArea(ingredients.Count);

        if (ingredients.Count == 0)
        {
            _messages.SetActive(true);
        }
        else
        {
            _messages.SetActive(false);
        }

        while (_slots.Count < ingredients.Count)
        {
            GameObject slotObj = ResourceManager.Instance.Instantiate("UI/UI_IngredientBoxSlot", _slotContainer);
            slotObj.transform.localScale = Vector3.one;
            _slots.Add(slotObj);
        }


        for (int i = 0; i < ingredients.Count; i++)
        {
            UI_IngredientBoxSlot slot = _slots[i].GetComponent<UI_IngredientBoxSlot>();
            slot.Clear();
            slot.Init(ingredients[i], ingredients[i].Quantity, OnSelectionChange);
        }

        _selectBtn.onClick.AddListener(SelectItem);
        _cancelBtn.onClick.AddListener(Cancel);
    }

    public void OnSelectionChange(UI_IngredientBoxSlot slot)
    {
        if (currentSlot == slot) return;
        if (currentSlot!= null) currentSlot.SetAsSelected(false);
        currentSlot = slot;
        currentSlot.SetAsSelected(true);

        if (!_selectBtn.interactable) _selectBtn.interactable = true;
    }

    public virtual void SelectItem()
    {
        if (currentSlot == null) return;
        OnSelection?.Invoke(currentSlot.ingredient);



        IngredientInfoData selectedData = _ingredientDatas.Find(i => i.DefaultData == currentSlot.ingredient);
        int idx = _ingredientDatas.IndexOf(selectedData);
        _playerData.SellIngredient(idx, 1, 0);

        currentSlot.data.Quantity--;
        if (currentSlot.data.Quantity <= 0)
        {
            List<IngredientInfoData> targetList = 
                currentSlot.data.DefaultData.Type == Enums.FoodType.Ricecake ? 
                _ricecakeIngredients : _teaIngredients;
            targetList.Remove(currentSlot.data);
        }

        CloseUI();
    }

    private void Cancel()
    {
        OnCancel?.Invoke();
        CloseUI();
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        OnUIClose?.Invoke();
        ResetSlots();
        base.CloseUI(isSound, true);
    }

    public UI_IngredientBoxSlot GetSlot(int idx)
    {
        if (idx < _slots.Count)
        {
            return _slots[idx].GetComponent<UI_IngredientBoxSlot>();
        }
        return null;
    }

    protected virtual void ResetSlots()
    {
        if (_slots == null) return;

        foreach (GameObject slot in _slots)
        {
            ResourceManager.Instance.Destroy(slot);
        }
        _slots.Clear();
        OnSelection = null;
        OnCancel = null;
        _selectBtn.onClick.RemoveAllListeners();
        _cancelBtn.onClick.RemoveAllListeners();

        if (currentSlot != null)
        {
            currentSlot.SetAsSelected(false);
            currentSlot = null;
        }
    }

    protected virtual void ResetData()
    {
        _gameManager = GameManager.Instance;
        _playerData = _gameManager.GetPlayerData();
        _ingredientDatas = _playerData.GetInventory<IngredientInfoData>();
        _ricecakeIngredients = new List<IngredientInfoData>();
        _teaIngredients = new List<IngredientInfoData>();

        bool[] _checked = new bool[_ingredientDatas.Count];

    
        for (int i = 0; i < _ingredientDatas.Count; i++)
        {
            if (_checked[i]) continue;

            IngredientInfoSO data = _ingredientDatas[i].DefaultData;
            int quantity = 0;
            for (int j = 0; j < _ingredientDatas.Count; j++)
            {
                if (_ingredientDatas[j].DefaultData == data)
                {
                    quantity += _ingredientDatas[j].Quantity;
                    _checked[j] = true;
                }
            }
            _checked[i] = true;

            IngredientInfoData newData = new IngredientInfoData(data, quantity);

            if (newData.DefaultData.Type == Enums.FoodType.Ricecake)
            {
                _ricecakeIngredients.Add(newData);
            }
            else if (newData.DefaultData.Type == Enums.FoodType.Tea)
            {
                _teaIngredients.Add(newData);
            }

            _ricecakeIngredients.Sort((a, b) => a.DefaultData.ID.CompareTo(b.DefaultData.ID));
             _teaIngredients.Sort((a, b) => a.DefaultData.ID.CompareTo(b.DefaultData.ID));
        }
    }

    private void SetContentArea(int ingredientsCount)
    {
        _slotContainer.TryGetComponent<RectTransform>(out RectTransform rect);

        Vector2 sizeDelta = rect.sizeDelta;

        int newHeight = _defaultContainerHeight;

        if (ingredientsCount > _maxVisibleContents)
        {
            newHeight += ((int)((ingredientsCount - 6) / 2) + 1) * _slotHeight;
        }

        sizeDelta.y = newHeight;

        rect.sizeDelta = sizeDelta;
    }
}
