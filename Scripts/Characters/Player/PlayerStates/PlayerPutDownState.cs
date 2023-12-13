using UnityEngine.InputSystem;

public class PlayerPutDownState : PlayerStayState
{
    private bool putDownConducted;
    public PlayerPutDownState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.PutDownParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        stateMachine.IsHolding = false;
        putDownConducted = false;
        StopAnimation(stateMachine.Player.AnimationData.PutDownParameterHash);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "PutDown");
        if (normalizedTime >= 0.3f && !putDownConducted)
        {
            stateMachine.Player.PutDown();
            putDownConducted = true;
        }
 
        if (normalizedTime >= 0.9f)
        {
            if (stateMachine.Player.Ingredient == null)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.PickUpState);
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
