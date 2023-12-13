using UnityEngine;

// 게임 이벤트가 일어날 때까지 기다려야 하는 경우는 배열에 이벤트 하나만 할당할 것
// 예외 처리를 제외하고 무조건 true를 반환하는 게임 이벤트만 배열에 여러 개 할당하기
public class TutorialEvent : TutorialBase
{
    [SerializeField] private TutorialEventOperator _tutorialGameOperator;
    [SerializeField] private TutorialEventType _eventType;

    public bool _isCleared = false;

    public override void Enter()
    {
        _isCleared = _tutorialGameOperator.SetNextEvent(_eventType);
    }

    public override void Execute(TutorialController controller)
    {
        if (_isCleared) controller.SetNextTutorial();
        else _isCleared = _tutorialGameOperator.SetNextEvent(_eventType);
    }

    public override void Exit()
    {
    }
}
