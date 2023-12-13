using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField] private string _targetTag;
    [SerializeField] private bool _isTrigger = false;

    public override void Enter()
    {
        _isTrigger = false;
    }

    public override void Execute(TutorialController controller)
    {
        if (_isTrigger) controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            _isTrigger = true;
        }
    }
}
