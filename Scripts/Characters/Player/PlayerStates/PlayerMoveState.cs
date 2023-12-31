public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationHash.MoveParameterHash);
        stateMachine.IsWalking = true;
        stateMachine.Player.ActivateParticle(true);
    }

    public override void Start()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationHash.MoveParameterHash);
    }

}
