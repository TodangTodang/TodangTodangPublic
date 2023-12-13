using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngredientBoxSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countTxt;
    [SerializeField] private GameObject _border;
    private Color _normalColor;
    private Color _selectedColor;

    private Action<UI_IngredientBoxSlot> OnSlotClicked;

    private Button button;

    public IngredientInfoData data;
    public IngredientInfoSO ingredient;
    public int index;

    public void Init(IngredientInfoData data, int count, Action<UI_IngredientBoxSlot> OnSlotSelection) 
    {
        ColorUtility.TryParseHtmlString("#FFF9E4", out _normalColor);
        ColorUtility.TryParseHtmlString("#DCECFF", out _selectedColor);

        _border.GetComponent<Image>().color = _normalColor;
        button = GetComponent<Button>();

        this.data = data;
        this.ingredient = data.DefaultData;
        _icon.sprite = ingredient.IconSprite;
        this._countTxt.text = count.ToString();   
        // TODO : changing icon
        button.onClick.AddListener(OnButtonClicked);
        OnSlotClicked += OnSlotSelection;
    }

    public void SetAsSelected(bool selected)
    {
        if (selected)
            _border.GetComponent<Image>().color = _selectedColor;
        else
            _border.GetComponent<Image>().color = _normalColor;
    }

    private void OnButtonClicked()
    {
        OnSlotClicked?.Invoke(this);
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON); 
    }

    public void Clear()
    {
        OnSlotClicked = null;
        if (button != null) button.onClick.RemoveAllListeners();
    }
}
