using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldAndWalkState : PlayerMoveState
{
    public PlayerHoldAndWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.HoldAndWalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.HoldAndWalkParameterHash);
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

    private void OnStay()
    {
        stateMachine.ChangeState(stateMachine.HoldState);
    }

    protected override void OnPickUp(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.PutDownState);
    }

}
