using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerStayState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationHash.IdleParameterHash);
    }


    public override void Start()
    {
        
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

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationHash.IdleParameterHash);
    }

    private void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.IsInteractable())
            stateMachine.ChangeState(stateMachine.InteractionState);
    }

}
