using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Market : UI_Base
{
    [SerializeField] private GameObject _marKetItemContent;

    [SerializeField] private Button[] _tabBtn;
    [SerializeField] private Sprite[] _defaultTabSprite;
    [SerializeField] private Sprite[] _selectedTabSprite;
    [SerializeField] private Button _sortBtn;
    [SerializeField] private TextMeshProUGUI _sortBtnText;
    private UI_MarketItemSlot[] _marketItemSlots;

    [SerializeField] private GameObject _cartContent;
    [SerializeField] private TextMeshProUGUI _totalOrderAmountText;
    [SerializeField] private TextMeshProUGUI _playerMoneyText;
    [SerializeField] private Button _orderBtn;
    [SerializeField] private Button _exitBtn;
    private UI_CartSlot[] _cartSlots;

    private SoundManager _soundManager;
    private ResourceManager _resourceManager;
    private GameManager _gameManager;
    private DataManager _dataManager;
    private UIManager _uiManager; 

    private Enums.MarketTabType _selectedTab = Enums.MarketTabType.Total;

    private int _playerMoney;
    private int _totalOrderAmount;

    public event Action OnOrderSubmitted;
    public event Action<Enums.MarketTabType> OnChangeTab;
    public event Action OnSort; 
    public event Action<bool> OnTutorialClosed;

    public bool IsAscendingOrder { get; private set; } = true; 

    private void Awake()
    {
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_resourceManager == null) _resourceManager = ResourceManager.Instance;
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_dataManager == null) _dataManager = DataManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;


        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
        Debug.Assert(_resourceManager != null, "Null Exception : ResourceManager");
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");

        InitBind();
    }

    private void Start()
    {
        _playerMoney = _gameManager.GetPlayerData().Money;
        _totalOrderAmount = 0;

        _playerMoneyText.text = _playerMoney.ToString();
        _totalOrderAmountText.text = _totalOrderAmount.ToString();

        _sortBtnText.text = Strings.Market.ASCENDINGSORT_TEXT;
        _orderBtn.interactable = false;

        SetSelectedTab(Enums.MarketTabType.Total);
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        _uiManager.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.Market.EXITCONFIRM
                , confirmCallback: Exit
                , cancelCallback: null
                )
            ); ;
    }

    // 이벤트 바인딩
    private void InitBind()
    {
        for (int i = 0; i < _tabBtn.Length; i++)
        {
            int index = i;
            _tabBtn[i].onClick.AddListener(() => OnTabButton((Enums.MarketTabType)index));
        }

        _sortBtn.onClick.AddListener(OnSortButton);
        _orderBtn.onClick.AddListener(OnOrderButton);
        _exitBtn.onClick.AddListener(() => CloseUI(true));
    }

    // Slot들 초기화 
    public void InitIngredientSlots(List<IngredientInfoData> data)
    {
        int dataCount = data.Count; 
        _marketItemSlots = new UI_MarketItemSlot[dataCount];
        _cartSlots = new UI_CartSlot[dataCount];

        for (int i = 0; i < dataCount; i++)
        {
            InitializeMarketItemSlot(data[i], i);
            InitializeCartSlot(data[i], i);
        }
    }

    // 재료 슬롯 초기화 
    private void InitializeMarketItemSlot(IngredientInfoData data, int index)
    {
        GameObject marketItemSlot = _resourceManager.Instantiate(Strings.Prefabs.UI_MARKETITEMSLOT, _marKetItemContent.transform);
        _marketItemSlots[index] = marketItemSlot.GetComponent<UI_MarketItemSlot>();

        _marketItemSlots[index].Initialize(data, index);
    }

    // 주문 내역 슬롯 초기화
    private void InitializeCartSlot(IngredientInfoData data, int index)
    {
        GameObject cartSlot = _resourceManager.Instantiate(Strings.Prefabs.UI_CARTSLOT, _cartContent.transform);
        _cartSlots[index] = cartSlot.GetComponent<UI_CartSlot>();
        _cartSlots[index].Initialize(data, index);

        _cartSlots[index].OnDeleteBtn += () =>
        _marketItemSlots[index].UpdateQuantity(-_marketItemSlots[index].CurrentQuantity);
        _marketItemSlots[index].OnQuantityChange += () => _cartSlots[index].UpdateQuantity(_marketItemSlots[index].CurrentQuantity);
    }

    // 총 주문 금액 업데이트 
    public void UpdateTotalOrderAmount(int totalOrderAmount)
    {
        _totalOrderAmount = totalOrderAmount; 
        _totalOrderAmountText.text = totalOrderAmount.ToString();
        _orderBtn.interactable = !SetOrderButtonActivation();
    }

    // 주문 버튼 활성화 여부 설정 
    private bool SetOrderButtonActivation()
    {
        bool active = false; 
        foreach(var slot in _cartSlots)
        {
            if (slot.gameObject.activeSelf)
            {
                active = true;
                break;
            }
        }

        return !active || _playerMoney < _totalOrderAmount; 
    }

    // 탭 버튼 클릭 시 호출
    private void OnTabButton(Enums.MarketTabType tabType)
    {
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        SetSelectedTab(tabType);
        OnChangeTab.Invoke(tabType); 
    }

    // 선택된 탭 설정
    private void SetSelectedTab(Enums.MarketTabType tabType)
    {
        ChangeTabImage(_selectedTab, _defaultTabSprite);
        _selectedTab = tabType;
        ChangeTabImage(tabType, _selectedTabSprite);
    }

    // 탭 이미지 변경 
    private void ChangeTabImage(Enums.MarketTabType tabType, Sprite[] newImage)
    {
        _tabBtn[(int)tabType].GetComponent<Image>().sprite = newImage[(int)tabType];
    }

    // 정렬 버튼 클릭 시 호출
    private void OnSortButton()
    {
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        IsAscendingOrder = !IsAscendingOrder;

        OnSort.Invoke();

        _sortBtnText.text = IsAscendingOrder == true ? Strings.Market.ASCENDINGSORT_TEXT : Strings.Market.DESCENDINGSORT_TEXT;
    }
    
    // 주문하기 버튼 클릭 시 호출
    private void OnOrderButton()
    {
        if (_playerMoney >= _totalOrderAmount)
        {
            UIManager.Instance.ShowPopup<UI_DefaultPopup>(
                new PopupParameter(
                    content: Strings.Market.ORDERCONFIRM
                    , confirmCallback: Order
                    , cancelCallback: null
                    )
                );
        }
    }

    // 주문 처리 
    private void Order()
    {
        _soundManager.Play(Strings.Sounds.UI_BUYANDSELL);
        OnOrderSubmitted?.Invoke();
        Exit();
    }

    // 종료 처리 
    private void Exit()
    {
        OnTutorialClosed?.Invoke(true);

        _gameManager.ChangeState(Enums.PlayerDayCycleState.DayEnd);
        _dataManager.SaveAllData();
        base.CloseUI();
    }
    
    public UI_MarketItemSlot[] GetMarketItemSlots()
    {
        return _marketItemSlots; 
    }

    public List<UI_CartSlot> GetCartSlots()
    {
        List<UI_CartSlot> cartSlot = new List<UI_CartSlot>();
        foreach (var slot in _cartSlots)
        {
            if (slot.gameObject.activeSelf)
            {
                cartSlot.Add(slot);
            }
        }

        return cartSlot;
    }
}