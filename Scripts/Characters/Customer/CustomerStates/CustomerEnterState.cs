
using UnityEngine;
using UnityEngine.AI;

public class CustomerEnterState : CustomerBaseState
{
    public CustomerEnterState(CustomerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_customer.AnimationData.moveParameterHash);
        SetDestination(_customer.GetSeatPos());
    }

    public override void Exit()
    {
        StopAnimation(_customer.AnimationData.moveParameterHash);
    }

    public override void Update()
    {
       StopWalk();
    }

    public void StopWalk()
    {
        if (_customer.arrived)
        {
            if (_agent.remainingDistance <= 0.1f)
            {
                StateMachine.ChangeState(StateMachine.WaitSeatState);
            }
        }
    }
}

