using UnityEngine.InputSystem;

public class PlayerStayState : PlayerBaseState
{
    public PlayerStayState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.StayParameterHash);
        stateMachine.IsWalking = false;
        stateMachine.Player.ActivateParticle(false);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.StayParameterHash);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
    }

    protected override void OnPickUp(InputAction.CallbackContext context)
    {
        base.OnPickUp(context);
        if (!stateMachine.IsHolding)
        {
            stateMachine.Player.PickUp();
            stateMachine.ChangeState(stateMachine.PickUpState);
        }
    }
}
