using UnityEngine;

public class TutorialControlPlayerState : TutorialBase
{
    [SerializeField] private TutorialGameEventOperator _tutorialGameOperator;
    [SerializeField] private bool _playerMovable = false;
    [SerializeField] private bool _playerInteractable = false;
    [SerializeField] private bool _playerPickable = false;
    [SerializeField] private bool _isTabClickable = false;
    [SerializeField] private bool _isRotate = false;
    [SerializeField] private Transform _targetTransform;

    private bool _isCleared = false;

    public override void Enter()
    {
        _isCleared = _tutorialGameOperator.ChangePlayerInputState(TutorialPlayerState.Move, _playerMovable);
        _isCleared = _tutorialGameOperator.ChangePlayerInputState(TutorialPlayerState.Interact, _playerInteractable);
        _isCleared = _tutorialGameOperator.ChangePlayerInputState(TutorialPlayerState.PickUp, _playerPickable);
        _isCleared = _tutorialGameOperator.ChangePlayerInputState(TutorialPlayerState.TabClickable, _isTabClickable);
        if (_isRotate) _isCleared = _tutorialGameOperator.ChangePlayerRotation(_targetTransform);
    }

    public override void Execute(TutorialController controller)
    {
        if (_isCleared) controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
}
