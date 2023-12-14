using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer: MonoBehaviour
{
    public GameObject _spawnObject { get; private set; }
    private CustomerData _data;
    private GameObject[] _menuIcon;
    
    public int PlacedIdx { get; private set; }
    
    private NavMeshAgent _agent;

    private Vector3 _seatPos;
    private Vector3 _exitPos;
    
    private Animator _animator;
    private CustomerStateMachine _customerStateMachine;
    private ResourceManager _resourceManager;
    public CustomerAnimationData AnimationData { get; private set; }
    private CustomerEnduranceController enduranceController;
    private OrderChatBubble _orderChat;
    private CustomerWear _wear;
    private CustomerEmotion _emotion;
    private CustomerRejectButton _rejectButton;
    
    [field:SerializeField] public List<IngredientInfoSO> OrderingMenu { get; private set; }
    private List<int> OrderingMenuPrice;
    private List<IngredientInfoSO> CorrectedMenu;
    private List<int> CorrectedMenuPrice;
    
    private Action<CustomerEmotionType,List<IngredientInfoSO>,int> OnDeliverMenu;
    
    private bool _isTimeOver = false;
    private int menuCount = 0;
    private int totalPay = 0;
    private int pay = 0;
    private int tip = 0;
    private float startTime;
    
    private float AddtionalPayRate;
    private Func<IEnumerator> OnGiveAfterOver;
    private Action OnReject;
    private List<RecipeInfoData> recipes;

    private bool _isCustomMode;
    private bool isGiveFood = false;
    public bool arrived { get; private set; }

    #region Initialize Method

    public void InitComponents()
    {
        TryGetComponent<NavMeshAgent>(out _agent);
        TryGetComponent<CustomerEnduranceController>(out enduranceController);
        TryGetComponent<OrderChatBubble>(out _orderChat);
        TryGetComponent<CustomerEmotion>(out _emotion);
        TryGetComponent<CustomerWear>(out _wear);
        TryGetComponent<Animator>(out _animator);
        TryGetComponent<CustomerRejectButton>(out _rejectButton);
        _resourceManager = ResourceManager.Instance;

        _data = new CustomerData();
        CorrectedMenu = new List<IngredientInfoSO>();
        OrderingMenu = new List<IngredientInfoSO>();
        OrderingMenuPrice = new List<int>();
        CorrectedMenuPrice = new List<int>();
        _customerStateMachine = new CustomerStateMachine(this);
        AnimationData = new CustomerAnimationData();
        AnimationData.Initialize();
        
        _isTimeOver = false;
        #if UNITY_EDITOR
        CheckValid();
        #endif
        _rejectButton.AddListener(CallOnReject);
        
    }
#if UNITY_EDITOR
    public void CheckValid()
    {
        DebugUtil.AssertNotAllocateInInspector(_agent,nameof(_agent));
        DebugUtil.AssertNotAllocateInInspector(_animator,nameof(_animator));
        DebugUtil.AssertNotAllocateInInspector(enduranceController,nameof(enduranceController));
        DebugUtil.AssertNotAllocateInInspector(_wear,nameof(_wear));
        DebugUtil.AssertNotAllocateInInspector(_emotion,nameof(_emotion));
        DebugUtil.AssertNotAllocateInInspector(_orderChat,nameof(_orderChat));
        DebugUtil.AssertNotAllocateInInspector(_rejectButton,nameof(_rejectButton));
        DebugUtil.AssertNullException(_resourceManager,nameof(_resourceManager));
    }
#endif
    public void InitPos(int placedIdx, Vector3 SeatPos, Vector3 ExitPos)
    {
        PlacedIdx = placedIdx;
        _seatPos = SeatPos;
        _exitPos = ExitPos;
    }

    public void InitCustomerData(CustomerDataSO dataSo, float extraEnduranceRate, float extraPayRate)
    {
        _data.Init(dataSo,extraEnduranceRate, extraPayRate);
    }

    public void InitChooseFood(Action<CustomerEmotionType, List<IngredientInfoSO>,int> OnDeliverToOperatorFood,IngredientInfoSO food,int foodPrice, IngredientInfoSO tea=null,int teaPrice = 0)
    {
        Sprite foodSprite = null;
        Sprite teaSprite = null;
        if (food != null)
        {
            OrderingMenu.Add(food);
            OrderingMenuPrice.Add(foodPrice);
            foodSprite = food.IconSprite;
        }
            
        if (tea != null)
        {
            OrderingMenu.Add(tea);
            OrderingMenuPrice.Add(teaPrice);
            teaSprite = tea.IconSprite;
        }

        menuCount = 0;
        totalPay = 0;
        pay = 0;
        tip = 0;
        isGiveFood = false;
        arrived = false;
        
        _orderChat.SetOrder(foodSprite,teaSprite);
        OnDeliverMenu = OnDeliverToOperatorFood;
    }

    public void StartCustomer(bool isCustomMode, GameObject spawnObj)
    {
        _spawnObject = spawnObj;
        _isCustomMode = isCustomMode;
        _customerStateMachine.Init();
    }

    public void Update()
    {
        _customerStateMachine.Update();
    }

    #endregion
    
    #region UserMethod
    
    public void Arrived(Func<IEnumerator> onGiveAfterOver,Action onReject)
    {
        OnGiveAfterOver = onGiveAfterOver;
        OnReject = null;
        OnReject += onReject;
        arrived = true;
    }
    
    private IEnumerator WaitOperation()
    {
        float waitTime = Time.time - startTime;
 
        while (waitTime < _data.currentMaxEndurance && !isGiveFood)
        {
            enduranceController.SetProgress(waitTime/_data.currentMaxEndurance);
            waitTime = Time.time - startTime;
            yield return null;
        }

        if (!isGiveFood)
        {
            _isTimeOver = true;
            
            yield return OnGiveAfterOver?.Invoke();
        
            _customerStateMachine.ChangeState(_customerStateMachine.ExitState);
        }
    }
    
    public void GetCorrectFoodMakeHappy()
    {
        startTime = Math.Min(startTime + 20, Time.time);
    }
    
    public void GiveFoods(bool isRight)
    {
        CustomerEmotionType currentEmotionType = GetEmotionType();
        
        ShowEmotionText(isRight,currentEmotionType);
        if(isRight)
        {
            PayCorrectFood();
            PayTip(currentEmotionType);
            ShowPayText();
            totalPay = pay + tip;
            _customerStateMachine.ChangeState(_customerStateMachine.SuccessState);
        }
        else
        {
            _customerStateMachine.ChangeState(_customerStateMachine.FailState);
        }

        ResetMenuLists();
    }
    
    public bool IsCorrectMenu(List<IngredientInfoSO> menu,out CustomerEmotionType currentEmotionType)
    {
        currentEmotionType = GetEmotionType();
        return menu.Count == OrderingMenu.Count 
               && CheckCorrectFood(menu) 
               && currentEmotionType != CustomerEmotionType.Angry;
    }
    
    private void ResetMenuLists()
    {
        CorrectedMenu.Clear();
        CorrectedMenuPrice.Clear();
        OrderingMenu.Clear();
        OrderingMenuPrice.Clear();
    }
    
    public void Exit()
    {
        _customerStateMachine.ChangeState(_customerStateMachine.ExitState);
    }

    public void StopWaiting(CustomerEmotionType type)
    {
        isGiveFood = true;
        enduranceController.SetActiveEnduranceUI(false,type);
    }
    
    public void CallOnReject()
    {
        OnReject?.Invoke();
    }
    
    public void AccessaryOff()
    {
        _wear.OffAccessary();
    }
    
    private void ShowPayText()
    {
        if (pay > 0)
            Invoke(nameof(PayTextOperation), 0.5f);
        if (tip > 0)
        {
            Invoke(nameof(TipTextOperation), 1f);
        }
    }

    private void PayTextOperation()
    {
        InstantiatePayText(pay);
    }
    
    private void TipTextOperation()
    {
        InstantiatePayText(tip);
    }

    private void InstantiatePayText(int pay)
    {
        GameObject payText = _resourceManager.Instantiate(Strings.Prefabs.UI_GOLD_TEXT);
        payText.transform.position = transform.position+new Vector3(0, 2, -2);
        if (payText.TryGetComponent(out UI_TextSetter moneyText))
        {
            moneyText.Init(pay.ToString());
        }
    }

    private void PayTip(CustomerEmotionType emotionType)
    {
        tip = 0;
        switch (emotionType)
        {
            case CustomerEmotionType.Perfect :
            case CustomerEmotionType.Great :
                tip = (int)(pay * _data.currentTipRates[(int)emotionType]);
                break;
        }
    }

    private void PayCorrectFood()
    {
        foreach (var price in CorrectedMenuPrice)
        {
            pay += (int)(price * _data.currentPayRate);
        }
    }
    // 수정 필요, 현지 수정한거 바탕으로 수정 가해야할 듯 
    private bool CheckCorrectFood(List<IngredientInfoSO> menu)
    {
        bool isRight = true;
        foreach (IngredientInfoSO food in menu)
        {
            int idx = OrderingMenu.FindIndex(x => x == food);
            if (idx != -1)
            {
                OrderingMenu.RemoveAt(idx);
                CorrectedMenuPrice.Add(OrderingMenuPrice[idx]);
                OrderingMenuPrice.RemoveAt(idx);
                CorrectedMenu.Add(food);
            }
            else
            {
                isRight = false;
            }
        }

        return isRight;
    }
    
    #endregion

    #region StateUseMethod

    public ICustomerState GetState()
    {
        return _customerStateMachine.GetCurrentState();
    }
    public Vector3 GetSeatPos()
    {
        return _seatPos;
    }

    public Vector3 GetExitPos()
    {
        return _exitPos;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return _agent;
    }

    public void OpenWaitUI()
    {
        enduranceController.SetActiveEnduranceUI(!_isCustomMode,GetEmotionType());
        _rejectButton.ActiveButton(!_isCustomMode);
        _orderChat.SetActive(true);
    }

    public void CloseWaitUI()
    {
        enduranceController.SetActiveEnduranceUI(false,CustomerEmotionType.Perfect);
        _rejectButton.ActiveButton(false);
        _orderChat.SetActive(false);
    }

    public void CallStartWaitOperation()
    {
        if (!_isCustomMode)
        {
            startTime = Time.time;
            StartCoroutine(WaitOperation());
        }
            
    }

    public void SetFace(Enums.FaceType type)
    {
        _wear.SetFace(type);
    }

    public void PlayEmotionEffect(bool isRight, CustomerEmotionType emotionType)
    {
        if (!isRight)
        {
            _emotion.PlayAngry();
            return;
        }
        
        switch (emotionType)
        {
            case CustomerEmotionType.Angry :
                _emotion.PlayAngry();
                break;
            case CustomerEmotionType.Perfect :
            case CustomerEmotionType.Great :
                _emotion.PlayHappy();
                break;
        }
    }

    public CustomerEmotionType GetEmotionType()
    {
        return enduranceController.GetCustomerEmotionType();
    }

    public void CallOnDeliverMenu(CustomerEmotionType Satisfy)
    {
        OnDeliverMenu?.Invoke(Satisfy,CorrectedMenu,totalPay);
    }

    public void InvokeExit()
    {
        Invoke("Exit", 2f);
    }

    public void ShowEmotionText(bool isRight, CustomerEmotionType currentEmotionType)
    {
        GameObject imageObj = _resourceManager.Instantiate(Strings.Prefabs.UI_EMOTION_TEST);
        imageObj.transform.position = transform.position+new Vector3(0, 2, -2);
        
        UI_ImageSetter imageSetter = null;
        
        if(imageObj.TryGetComponent(out imageSetter))
        {
            if (!isRight)
            {
                Sprite EmotionImage = enduranceController.GetCustomerEmotionSprite(CustomerEmotionType.Angry);
                Sprite EmotionText = _data.emotionImageText[(int)CustomerEmotionType.Angry]; 
                imageSetter.Init(EmotionText,EmotionImage);
            }
            else
            {
                Sprite EmotionImage = enduranceController.GetCustomerEmotionSprite(currentEmotionType);
                Sprite EmotionText = _data.emotionImageText[(int)currentEmotionType]; 
                imageSetter.Init(EmotionText,EmotionImage);
            }
        }
    }
    
    #endregion
}
