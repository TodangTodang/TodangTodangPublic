using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MarketItemSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _typeImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _currentQuantityText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _minusBtn;
    [SerializeField] private Button _plusBtn;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private TextMeshProUGUI _expirationText;

    private ResourceManager _resourceManager;

    public int CurrentQuantity { get; private set; }

    public int ItemIdx { get; set; }
    public int Price { get; private set; }

    public event Action OnQuantityChange;

    private void Awake()
    {
        InitBind();

        _resourceManager = ResourceManager.Instance;

        _quantityText.text = "0";
        CurrentQuantity = int.Parse(_quantityText.text);
    }

    private void InitBind()
    {
        _minusBtn.onClick.AddListener(() => UpdateQuantity(-1));
        _plusBtn.onClick.AddListener(() => UpdateQuantity(1));
    }

    public void Initialize(IngredientInfoData data, int index)
    {
        _icon.sprite = data.DefaultData.IconSprite;
        ItemIdx = index;
        _nameText.text = data.DefaultData.Name;
        _expirationText.text = $"유통기한 {data.DefaultData.BaseExpirationDate}일";
        _typeImage.sprite = GetFoodTypeIcon(data.DefaultData.Type);
        _priceText.text = data.PriceAtBuy.ToString();
        _currentQuantityText.text = $"보유 {(data.Quantity > 100 ? "100+" : data.Quantity)}";

        Price = int.Parse(_priceText.text); 
    }

    public void UpdateQuantity(int value)
    {
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);

        CurrentQuantity = Mathf.Clamp(CurrentQuantity + value, 0, 99);
        _quantityText.text = $"{CurrentQuantity}";

        OnQuantityChange.Invoke();
    }

    private Sprite GetFoodTypeIcon(Enums.FoodType type)
    {
        string iconPath = (type == Enums.FoodType.Ricecake) ? Strings.Sprites.MARKET_RICECAKE_TYPE_ICON : Strings.Sprites.MARKET_TEA_TYPE_ICON;
        return _resourceManager.LoadSprite(iconPath);
    }
}
