using System.Collections.Generic;
using UnityEngine;

public enum TutorialPlayerState
{
    Move,
    Interact,
    PickUp,
    TabClickable,
}

public class TutorialGameEventOperator : TutorialEventOperator
{
    #region Player Input String
    private const string MOVE = "Move";
    private const string INTERACTION = "Interaction";
    private const string PICKUP = "PickUp";
    private const string TAB_CLICKABLE = "Game/CookBook";
    #endregion

    [SerializeField] private GameOperator _gameOperator;
    [SerializeField] private Player _player;
    [SerializeField] private IngredientBox _ingredientBox;
    [SerializeField] private Transform _steamerFoodPos;
    [SerializeField] private Transform _pickUpTableFoodPos;
    [SerializeField] private GameObject _tutorialControllerGameFinish;

    private bool _steamerState = false;         // false : 찜기 안에 아무것도 없는 경우 , true : 찜기 안에 있는 경우
    private bool _pickUpTableState = false;

    #region Player Input Enum To String
    private Dictionary<TutorialPlayerState, string> playerInputToString = new Dictionary<TutorialPlayerState, string>()
        { 
            { TutorialPlayerState.Move, MOVE },
            { TutorialPlayerState.Interact, INTERACTION },
            { TutorialPlayerState.PickUp, PICKUP },
            { TutorialPlayerState.TabClickable, TAB_CLICKABLE },
        };
    #endregion

    private UI_IngredientBox _uiIngredientBox;

    private void Start()
    {
        _gameOperator.SetTotalCustomerCount(5);
        _gameOperator.OnGameFinishCallback += CallOnGameFinish;
    }

    public bool ChangePlayerInputState(TutorialPlayerState stateType, bool state)
    {
        if (_player == null || !playerInputToString.ContainsKey(stateType)) return false;
        string inputState = playerInputToString[stateType];
        _player.Input.ControlInput(inputState, state);
        return true;
    }

    #region Switch Game Events
    public override bool SetNextEvent(TutorialEventType eventType)
    {
        switch (eventType)
        {
            case TutorialEventType.GenerateTutorialCusotmer:
                return GenerateTutorialCustomer();
            case TutorialEventType.InteractWithIngredientBox:
                return InteractWithIngredientBox();
            case TutorialEventType.CheckSteamerState:
                return CheckSteamerState();
            case TutorialEventType.CheckFoodType:
                return CheckFoodType();
            case TutorialEventType.CheckPickUpTableState:
                return CheckPickUpTableState();
            case TutorialEventType.ActiveAutoCustomerGenerate:
                return ActiveAutoCustomerGenerate();
            default: 
                return false;
        }
    }
    #endregion

    public bool ChangePlayerRotation(Transform targetTransform)
    {
        if (_player == null) return false;
        Vector3 v = (targetTransform.position - _player.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(v);
        _player.transform.rotation = q;
        return true;
    }

    private bool GenerateTutorialCustomer()
    {
        if (_gameOperator == null) return false;
        _gameOperator.SpawnCustomer();
        return true;
    }

    private bool InteractWithIngredientBox()
    {
        if (_uiIngredientBox == null) _uiIngredientBox = UIManager.Instance.GetUIComponent<UI_IngredientBox>();
        if (_uiIngredientBox.gameObject.activeSelf) return true;
        else return false;
    }

    private bool CheckSteamerState()
    {
        if (_steamerFoodPos == null) return false;

        // 찜기 안에 재료가 있는 상태에서 재료를 꺼냈을 때 (백설기 완성)
        if (_steamerState && _steamerFoodPos.childCount == 0) 
        {
            _steamerState = !_steamerState;
            return true;
        }
        // 찜기 안에 재료가 없는 상태에서 재료를 넣었을 때 (쌀가루 넣기)
        else if (!_steamerState && _steamerFoodPos.childCount > 0)
        {
            _steamerState = !_steamerState;
            return true;
        }
        else return false;
    }

    private bool CheckFoodType()
    {
        if (_steamerFoodPos == null || _steamerFoodPos.childCount == 0) return false;
        if (_steamerFoodPos.GetChild(0).gameObject.name == "BaekSeolGi") return true;
        else return false;
    }

    private bool CheckPickUpTableState()
    {
        if (_pickUpTableFoodPos == null) return false;
        if (_pickUpTableState && _pickUpTableFoodPos.childCount == 0)
        {
            _pickUpTableState = !_pickUpTableState;
            return true;
        } 
        else if (!_pickUpTableState && _pickUpTableFoodPos.childCount > 0)
        {
            _pickUpTableState = !_pickUpTableState;
            return true;
        }
        else return false;
    }

    private bool ActiveAutoCustomerGenerate()
    {
        if (_gameOperator == null) return false;
        _gameOperator.EnableAutoCustomerGenerate();
        _gameOperator.StartClock();
        return true;
    }

    private void CallOnGameFinish()
    {
        _tutorialControllerGameFinish.SetActive(true);
    }
}
