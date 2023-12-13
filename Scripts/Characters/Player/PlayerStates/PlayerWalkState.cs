using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerMoveState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.MovementInput == Vector2.zero)
        {
            OnStay();
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    private void OnStay()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.IsInteractable())
            stateMachine.ChangeState(stateMachine.InteractionState);
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
