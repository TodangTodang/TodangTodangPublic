using System;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    [SerializeField] private UI_InventoryTab[] _tabList;
    [SerializeField] private UI_InventoryDetail _uiInventoryDetail;
    [SerializeField] private RectTransform _scrollViewContainer;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private TMP_Text _playerMoneyText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _emptyItemWarning;

    public event Action OnOpenInventory;
    public event Action<Enums.InventoryType> OnTabChanged;
    public event Action<int> OnDetailViewChanged;

    private float _scrollViewRectPosition;
    private int _curSelectedItemIdx;
    private Enums.InventoryType _curSelectedInventory = Enums.InventoryType.Ingredient;
    private UI_InventorySlot[] _slotList;

    private void Start()
    {
        #region Check Null Exception
#if UNITY_EDITOR
        Debug.Assert(_tabList.Length != 0, "Null Exception : _tabList");
        Debug.Assert(_uiInventoryDetail, "Null Exception : _uiInventoryDetail");
        Debug.Assert(_scrollViewContainer, "Null Exception : _scrollViewContainer");
        Debug.Assert(_scrollRect, "Null Exception : _scrollRect");
        Debug.Assert(_playerMoneyText, "Null Exception : _playerMoneyText");
        Debug.Assert(_closeButton != null, "Null Exception : _closeButton");
        Debug.Assert(_emptyItemWarning != null, "Null Exception : _emptyItemWarning");
#endif
        #endregion

        _scrollViewRectPosition = _scrollViewContainer.anchoredPosition.x;
        _closeButton.onClick.AddListener(() => CloseUI());
        InitTab();
    }

    // 인벤토리 UI를 열 때
    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
        _curSelectedItemIdx = 0;
        _tabList[0].CallOnTabClicked();              // 첫 번째 탭이 선택된 상태로 변경
        OnOpenInventory?.Invoke();
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        base.CloseUI(isSound, isAnimated);
    }

    public UI_InventoryTab[] GetTabList()
    {
        return _tabList;
    }

    public UI_InventorySlot[] GetSlotList()
    {
        return _slotList;
    }

    public UI_InventoryDetail GetUIInventoryDetail()
    {
        if (_uiInventoryDetail == null) return null;
        return _uiInventoryDetail;
    }

    // 전체 UI 갱신 (스크롤 뷰, 슬롯, 디테일 뷰)
    public void RefreshScrollView(int dataCnt)
    {
        _curSelectedItemIdx = 0;                // 선택된 슬롯 인덱스 0으로 초기화
        InitSlots(dataCnt);                     // 전체 슬롯 갱신

        // 데이터가 없다면 (인벤토리가 비어있다면)
        if (dataCnt == 0)
        {
            _emptyItemWarning.SetActive(true);                     // 아이템이 없다는 이미지 활성화
            _uiInventoryDetail.gameObject.SetActive(false);     // 디테일 뷰 비활성화
        }
        // 현재 인벤토리에 아이템이 있을 때
        else
        {
            _emptyItemWarning.SetActive(false);                    // 아이템이 없다는 이미지 비활성화
            _uiInventoryDetail.gameObject.SetActive(true);      // 디테일 뷰 활성화
            _slotList[0].CallOnClicked();                       // 첫 번째 슬롯 선택
        }
    }

    // 유저 정보 UI 업데이트 (돈)
    public void UpdatePlayerMoneyUI(int playerMoney, bool isInit = false)
    {
        if (!isInit)
        {
            string curMoenyStr = _playerMoneyText.text.Replace(",", String.Empty);
            if (int.TryParse(curMoenyStr, out int curMoney))
            {
                UIEffect.EmphasizeText(
                    _playerMoneyText
                    , curMoney, playerMoney
                    , Colors.TextBlue
                    , (curMoney < playerMoney) ? Colors.MoonYellow : Colors.Pink);
            }
        }
        _playerMoneyText.text = playerMoney.ToString("N0");
    }

    // UI가 처음 생성됐을 때 Tab 리스트 초기 설정
    private void InitTab()
    {
        for (int i = 0; i < _tabList.Length; i++)
        {
            _tabList[i].SetTabSelectedState(false);                     // 모든 탭 unselected 상태로 변경
            _tabList[i].OnClicked += ChangeTab;                         // 탭 클릭 이벤트에 UI의 탭 변경 메서드 연결
        }
        _tabList[0].CallOnTabClicked();              
    }

    // 탭 변경 메서드
    private void ChangeTab(Enums.InventoryType type)
    {   
        // 스크롤 뷰 맨 위로 이동
        // 이전에 선택되어 있던 탭을 unselected 상태로 변경 후 현재 선택된 탭의 타입으로 인벤토리 타입 변경
        _scrollViewContainer.anchoredPosition = new Vector3(_scrollViewRectPosition, 0, 0);
        _tabList[(int)_curSelectedInventory].SetTabSelectedState(false);
        _curSelectedInventory = type;
        OnTabChanged?.Invoke(type);
    }

    // 슬롯 초기화
    private void InitSlots(int dataCnt)
    {
        // 전체 슬롯 개수 (스크롤 뷰 자식에 존재하는 슬롯 프리팹 수)
        int slotCnt = _scrollViewContainer.childCount;          

        // 슬롯 수가 아이템 수보다 적을 경우
        while (slotCnt < dataCnt)
        {
            GameObject go = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_INVENTORY_SLOT, _scrollViewContainer);
            go.transform.localScale = Vector3.one;
            slotCnt = _scrollViewContainer.childCount;
        }

        // 슬롯 리스트 초기화
        _slotList = new UI_InventorySlot[dataCnt];
        for (int i = 0; i < dataCnt; i++)
        {
            GameObject slotObj = _scrollViewContainer.GetChild(i).gameObject;
            slotObj.SetActive(true);
            _slotList[i] = slotObj.GetComponent<UI_InventorySlot>();
            _slotList[i].SetScrollRect(_scrollRect);
            _slotList[i].SetSlotSelected(false);            // 슬롯 선택 해제 상태로 설정
            _slotList[i].OnClicked += OnSelectedSlotChanged;
        }

        for (int i = 0; i <  (slotCnt - dataCnt); i++)
        {
            GameObject go = _scrollViewContainer.GetChild(dataCnt).gameObject;
            ResourceManager.Instance.Destroy(go);
        }
    }

    // 슬롯 클릭 이벤트에 연결
    private void OnSelectedSlotChanged(UI_InventorySlot slot)
    {
        // 이전에 선택된 슬롯 선택 해제 후 현재 선택된 슬롯 인덱스 값 업데이트
        _slotList[_curSelectedItemIdx].SetSlotSelected(false);
        _curSelectedItemIdx = slot.ItemIdx;
        OnDetailViewChanged?.Invoke(_curSelectedItemIdx);
    }
}
