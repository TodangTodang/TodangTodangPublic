using UnityEngine;

public class PlayerStateMachine : StateMachine 
{
    public Player Player { get; }

    //Stay States
    public PlayerStayState StayState { get; }
    public PlayerIdleState IdleState { get; }
    public PlayerPickUpState PickUpState { get; }
    public PlayerPutDownState PutDownState { get; }
    public PlayerInteractionState InteractionState { get; }
    public PlayerHoldState HoldState { get; }

    //Move States
    public PlayerMoveState MoveState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerHoldAndWalkState HoldAndWalkState { get; }

    public Transform MainCameraTransform { get; set; }
    public Vector2 MovementInput { get; set; }
    public bool IsWalking { get; set; }
    public bool IsHolding { get; set; }

    public float InteractionTime { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        MainCameraTransform = Camera.main.transform;

        IsWalking = false;
        IsHolding = false;

        StayState = new PlayerStayState(this);
        IdleState = new PlayerIdleState(this);
        PickUpState = new PlayerPickUpState(this);
        PutDownState = new PlayerPutDownState(this);
        InteractionState = new PlayerInteractionState(this);
        HoldState = new PlayerHoldState(this);

        MoveState = new PlayerMoveState(this);
        WalkState = new PlayerWalkState(this);
        HoldAndWalkState = new PlayerHoldAndWalkState(this);
    }
}
