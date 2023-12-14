using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionState : PlayerStayState
{
    public PlayerInteractionState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.Interaction();
        if (stateMachine.Player.Ingredient != null)
        {
            stateMachine.ChangeState(stateMachine.HoldState);
            return;
        }
        stateMachine.InteractionTime = 1f;
        StartAnimation(stateMachine.Player.AnimationHash.InteractionParameterHash);
        Debug.Log("InteractionState");
    }

    public override void Update()
    {
        base.Update();
        stateMachine.InteractionTime -= Time.deltaTime;
        if (stateMachine.InteractionTime < 0f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    
    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationHash.InteractionParameterHash);
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.IsAtPlaceable() && stateMachine.Player.IsInteractable()) return;
        stateMachine.InteractionTime = 1f;
        stateMachine.Player.Interaction();
    }

    protected override void Move()
    {
        
    }

    protected override void OnPickUp(InputAction.CallbackContext context)
    {
        
    }
}
