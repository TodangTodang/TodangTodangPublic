using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUpState : PlayerStayState
{
    public PlayerPickUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.IsHolding = true;
        StartAnimation(stateMachine.Player.AnimationHash.PickUpParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationHash.PickUpParameterHash);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "PickUp");
        if (normalizedTime >= 0.9f)
        {
            if (stateMachine.Player.Ingredient == null)
            {
                stateMachine.IsHolding = false;
                if (stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>() != Vector2.zero) 
                {
                    stateMachine.ChangeState(stateMachine.WalkState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.IdleState);
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.HoldState);
            }
        }
    }

    public override void FixedUpdate()
    {
        
    }


    protected override void Move()
    {

    }

    protected override void OnPickUp(InputAction.CallbackContext context)
    {

    }

}
