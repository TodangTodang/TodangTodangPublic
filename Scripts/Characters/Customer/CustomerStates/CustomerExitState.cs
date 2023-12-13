public class CustomerExitState : CustomerBaseState
{
    public CustomerExitState(CustomerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _customer.GetNavMeshAgent().isStopped = false;
        _customer.GetNavMeshAgent().angularSpeed = 360;
        StartAnimation(_customer.AnimationData.moveParameterHash);
        SetDestination(_customer.GetExitPos());
    }

}