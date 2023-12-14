using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecipeSlot : MonoBehaviour
{
    [SerializeField] private Image _outerbackground;
    [SerializeField] private Image _innerBackground;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _new;

    private Color _outerColor;
    private Color _iconColor;

    private Color _innerNormal;
    private Color _innerSelected;

    private Button _slotBtn;
    private event Action<UI_RecipeSlot> _onSlotSelection;
    private event Action _updateNewMark;

    [HideInInspector]
    public CookBookSO recipe;


    private void Start()
    {
        _slotBtn = GetComponent<Button>();
        _slotBtn.onClick.AddListener(ClickSlot);
    }

    public void Init(CookBookSO recipe, Action<UI_RecipeSlot> OnSlotSelection, Action UpdateNewMark, bool isNew)
    {
        SetColor();

        this.recipe = recipe;

        _outerColor.a = 0;
        _iconColor.a = 0.5f;
        _outerbackground.color = _outerColor;
        _innerBackground.color = _innerNormal;
        _icon.sprite = this.recipe.Result.IconSprite;
        _icon.color = _iconColor;
        _onSlotSelection += OnSlotSelection;
        _updateNewMark += UpdateNewMark;
        SetNewMark(isNew);
    }

    public void Init(CookBookSO recipe, Sprite icon, Action<UI_RecipeSlot> OnSlotSelection, Action UpdateNewMark, bool isNew)
    {
        SetColor();

        this.recipe = recipe;

        _outerColor.a = 0;
        _iconColor.a = 0.5f;
        _outerbackground.color = _outerColor;
        _innerBackground.color = _innerNormal;
        _icon.sprite = icon;
        _icon.color = _iconColor;
        _onSlotSelection += OnSlotSelection;
        _updateNewMark += UpdateNewMark;
        SetNewMark(isNew);
    }

    private void ClickSlot()
    {
        _onSlotSelection?.Invoke(this);
        
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);

        if (_new.activeInHierarchy)
        {
            
            GameManager.Instance.GetPlayerData().RemoveUnlockedRecipe(recipe.Result as ResultInfoSO);
            _updateNewMark?.Invoke();
            SetNewMark(false);
        }
    }

    public void SelectRecipe(bool selected)
    {
        _outerColor.a = selected ? 1 : 0;
        _iconColor.a = selected ? 1 : 0.5f;
        _outerbackground.color = _outerColor;
        _icon.color = _iconColor;
        _innerBackground.color = selected ? _innerSelected : _innerNormal;
    }

    private void SetColor()
    {
        ColorUtility.TryParseHtmlString("#E2DECE", out _outerColor);
        ColorUtility.TryParseHtmlString("#E7E7E7", out _innerNormal);
        ColorUtility.TryParseHtmlString("#FFE4AF", out _innerSelected);
        _iconColor = Color.white;
    }

    public void SetNewMark(bool isNew)
    {
        _new.SetActive(isNew);
    } 
}
