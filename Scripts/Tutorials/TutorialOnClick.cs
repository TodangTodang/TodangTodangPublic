public class TutorialOnClick : TutorialBase
{
    public bool IsClicked = false;

    public override void Enter()
    {
    }

    public override void Execute(TutorialController controller)
    {
        if (IsClicked) controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
}
