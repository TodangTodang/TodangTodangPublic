using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldState : PlayerStayState
{
    public PlayerHoldState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.HoldParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.HoldParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.MovementInput != Vector2.zero)
        {
            OnMove();
            return;
        }
    }

    public override void FixedUpdate()
    {
        
    }

    protected override void OnPickUp(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.PutDownState);
    }

    private void OnMove()
    {
        stateMachine.ChangeState(stateMachine.HoldAndWalkState);
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.IsInteractable())
            stateMachine.Player.Interaction();

    }
}
