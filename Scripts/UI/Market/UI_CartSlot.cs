using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CartSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _deleteBtn;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _totalPriceText;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private SoundManager _soundManager; 

    private int _basePrice; 
    public event Action OnDeleteBtn;
    
    public int ItemIdx { get; set; }

    public int Quantity { get; private set; }

    private void Awake()
    {
        if(_soundManager == null) _soundManager = SoundManager.Instance;
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");

        _deleteBtn.onClick.AddListener(OnDeleteButtonClick);
    }

    public void Initialize(IngredientInfoData data, int index)
    {
        gameObject.SetActive(false);

        _icon.sprite = data.DefaultData.IconSprite; 
        ItemIdx = index;
        _nameText.text = data.DefaultData.Name;
        _quantityText.text = "0";
        _basePrice = data.PriceAtBuy; 
        _totalPriceText.text = _basePrice.ToString();
    }

    private void OnDeleteButtonClick()
    {
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        OnDeleteBtn.Invoke(); 
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        int totalPrice = newQuantity * _basePrice;

        _quantityText.text = Quantity.ToString();
        _totalPriceText.text = totalPrice.ToString();

        if (!gameObject.activeSelf)
        {
            transform.SetAsLastSibling();
        }

        gameObject.SetActive(Quantity > 0);
    }
}