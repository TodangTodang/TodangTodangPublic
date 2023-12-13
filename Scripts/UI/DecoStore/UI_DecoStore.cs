using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class UI_DecoStore : UI_Base
{
    public event Action OnOpenDecoStore;
    public event Action<int> OnBuyStoreDeco;

    [SerializeField] private RectTransform _scrollViewContainer;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _moneyText;

    private int _playerMoney;
    private UI_DecoStoreSlot[] _slotList;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() => CloseUI(true));
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
        OnOpenDecoStore?.Invoke();
    }

    public void UpdatePlayerMoney(int money, bool isInit = false)
    {
        if (!isInit)
        {
            string curMoenyStr = _moneyText.text.Replace(",", String.Empty);
            if (int.TryParse(curMoenyStr, out int curMoney))
            {
                UIEffect.EmphasizeText(
                    _moneyText
                    , curMoney, money
                    , Colors.TextBlue
                    , (curMoney < money) ? Colors.MoonYellow : Colors.Pink);
            }
        }
        _moneyText.text = money.ToString("N0");
        _playerMoney = money;
    }

    public void RefreshUI(List<StoreDecorationInfoData> datas)
    {
        int dataCnt = datas.Count;
        int slotCnt = _scrollViewContainer.childCount;          // 전체 슬롯 개수 (스크롤 뷰 자식에 존재하는 슬롯 프리팹 수)

        // 슬롯 수가 아이템 수보다 적을 경우
        while (slotCnt < dataCnt)
        {
            GameObject go = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_DECOSTORE_SLOT, _scrollViewContainer);
            go.transform.localScale = Vector3.one;
            slotCnt = _scrollViewContainer.childCount;
        }

        _slotList = new UI_DecoStoreSlot[dataCnt];
        for (int i = 0; i < dataCnt; i++)
        {
            GameObject slotObj = _scrollViewContainer.GetChild(i).gameObject;
            slotObj.transform.localScale = Vector3.one;
            _slotList[i] = slotObj.GetComponent<UI_DecoStoreSlot>();
            _slotList[i].Init(datas[i], _playerMoney);      // 슬롯 정보 세팅
            _slotList[i].SetClickAction(CallOnBuyStoreDeco);
        }

        for (int i = 0; i < (slotCnt - dataCnt); i++)
        {
            GameObject go = _scrollViewContainer.GetChild(dataCnt).gameObject;
            ResourceManager.Instance.Destroy(go);
        }
    }

    private void CallOnBuyStoreDeco(int id)
    {
        OnBuyStoreDeco?.Invoke(id);

        SoundManager.Instance.Play(Strings.Sounds.UI_BUYANDSELL); 
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        base.CloseUI(isSound, true);
    }
}
