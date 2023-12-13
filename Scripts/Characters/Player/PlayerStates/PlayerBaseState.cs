using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    private Transform playerTransform;

    public float footstepInterval = 0.3f;
    private float lastFootstepTime;

    private Vector3 targetPosition;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerTransform = stateMachine.Player.transform;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void FixedUpdate()
    {
        RigidBodyMove();
    }

    private void RigidBodyMove()
    {
        
        Player player = stateMachine.Player;
        Rigidbody rigid = player.GetRigidBody();
        Vector3 movementDirection = GetMovementDirection();

        if (movementDirection == Vector3.zero)
        {
            //lastFootstepTime = Time.fixedTime; 
            return;
        }
        float movementSpeed =player.movementSpeed;
        Vector3 targetPosition = player.transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime;
        //Vector3 newPosition = Vector3.Slerp(stateMachine.Player.gameObject.transform.position, targetPosition, movementSpeed * Time.fixedDeltaTime);
        //newPosition.y = 0;
        
        
        float rotationSpeed = 5.0f;
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        Quaternion newRotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        
        rigid.MoveRotation(newRotation);
        rigid.MovePosition(targetPosition);
        
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    protected virtual void AddInputActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Interaction.performed += OnInteraction;
        stateMachine.Player.Input.PlayerActions.PickUp.performed += OnPickUp;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Interaction.performed -= OnInteraction;
        stateMachine.Player.Input.PlayerActions.PickUp.performed -= OnPickUp;
    }

    protected virtual void OnInteraction(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnPickUp(InputAction.CallbackContext context)
    {

    }


    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    protected virtual void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Player player = stateMachine.Player;
   
        if (movementDirection == Vector3.zero)
        {
            lastFootstepTime = Time.time; 
            return;
        }

        // float rotationSpeed = 5.0f;
        // Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        // Quaternion newRotation = Quaternion.Slerp(stateMachine.Player.gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // player.transform.rotation = newRotation;

        if (Time.time - lastFootstepTime > footstepInterval)
        {
            int num = Random.Range(1, 3);
            SoundManager.Instance.Play(Strings.Sounds.PLAYER_FOOTSTEP + num);
            lastFootstepTime = Time.time;
        }

    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        forward.y = 0;     
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
