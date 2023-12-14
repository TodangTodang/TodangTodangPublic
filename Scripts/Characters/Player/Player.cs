using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerAnimationHash AnimationHash { get; private set; }
    public PlayerInput Input { get; private set; }
    public Animator Animator { get; private set; }
    private Rigidbody Rigidbody;

    [field: SerializeField] public Transform foodPos { get; private set; }

    [field: SerializeField] public GameObject Ingredient { get; set; }

    [field: SerializeField] public float movementSpeed { get; private set; }
    private PlayerStateMachine stateMachine;

    [SerializeField] private LayerMask interactionLayer;
    private IInteractable currentInteraction;
    private IInteractable prevInteraction;

    public event Action OnInteractionState;
    public event Action OnPickUpState;
    public event Action OnPutDownState;

    private Action InteractionAction;
    private Action PickUpAction;
    private Action PutDownAction;

    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 boxHalfSize;
    [SerializeField] private float rayCastLength;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private ParticleSystem walkParticle;

    private void Awake()
    {
        AnimationHash.Initialize();
        Input = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
        TryGetComponent<Rigidbody>(out Rigidbody);
        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        ResetState();
    }
    
    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
        DetectInteractionObject();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void DetectInteractionObject()
    {
        RaycastHit hit;
        
        if (Physics.BoxCast(transform.position + startOffset, boxHalfSize, transform.forward, out hit, Quaternion.identity, rayCastLength, interactionLayer))
        {
            endPos = hit.point;

            if (!hit.transform.TryGetComponent<IInteractable>(out currentInteraction)) return;
            if (currentInteraction == prevInteraction) return;

            currentInteraction.SetPlayer(this);

            if (currentInteraction.CanInteractWithPlayer)
            {
                InteractionAction = currentInteraction.Interaction;
            }
            else
            {
                InteractionAction = null;
            }

            if (currentInteraction.IsPlaceable)
            {
                PickUpAction = currentInteraction.PickUp;
                PutDownAction = currentInteraction.PutDown;
            }
            else
            {
                PickUpAction = null;
                PutDownAction = null;
            }

            SelectObject();
        }
        else
        {
            endPos = Vector3.zero;
            if (currentInteraction != null)
            {
                currentInteraction.SelectObject(false);
                currentInteraction = null;
                prevInteraction = null;
            }
            InteractionAction = null;
            PickUpAction = null;
            PutDownAction = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPo = transform.position + startOffset;
        if (endPos == Vector3.zero)
        {
            endPos = startPo + transform.forward * 2f;    
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPo,endPos);
        Gizmos.DrawWireCube(endPos,boxHalfSize*2f);
    }

    private void SelectObject()
    {
        if (currentInteraction != prevInteraction)
        {
            if (prevInteraction != null)
                prevInteraction.SelectObject(false);
            currentInteraction.SelectObject(true);
            prevInteraction = currentInteraction;
        }
    }

    public void Interaction()
    {
        InteractionAction?.Invoke();
    }

    public void PickUp()
    {
        PickUpAction?.Invoke();
    }

    public void PutDown()
    {
        PutDownAction?.Invoke();
    }

    public bool IsAtPlaceable()
    {
        if (PickUpAction != null) return true;
        return false;
    }

    public bool IsInteractable()
    {
        if (Input.IsMouseOverUIButton()) return false;
        if (InteractionAction != null) return true;
        return false;
    }

    public void ResetState()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void ActivateParticle(bool isActive)
    {
        if (isActive)
        {
            walkParticle.Play();
        }
        else
        {
            walkParticle.Stop();
        }
    }

    #region GetterMethod

    public Rigidbody GetRigidBody()
    {
        return Rigidbody;
    }

    #endregion
}
