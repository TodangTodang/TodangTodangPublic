using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
    [SerializeField] private bool _isTimeStop = false;
    private DialogSystem _dialogSystem;

    public override void Enter()
    {
        _dialogSystem = GetComponent<DialogSystem>();
        _dialogSystem.Init();
        if (_isTimeStop) Time.timeScale = 0f;
    }

    public override void Execute(TutorialController controller)
    {
        bool isCompleted = _dialogSystem.UpdateDialog();

        if (isCompleted) controller.SetNextTutorial();
    }

    public override void Exit()
    {
        _isTimeStop = false;
        Time.timeScale = 1f;
    }
}
