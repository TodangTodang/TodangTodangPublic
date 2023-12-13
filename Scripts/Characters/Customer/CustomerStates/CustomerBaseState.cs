
using UnityEngine;
using UnityEngine.AI;

public class CustomerBaseState : ICustomerState
{
    public CustomerStateMachine StateMachine;
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected Customer _customer;
    public CustomerBaseState(CustomerStateMachine stateMachine)
    {
        
#if UNITY_EDITOR
        DebugUtil.AssertNullException(stateMachine != null,nameof(_customer));
#endif
        
        StateMachine = stateMachine;
        _customer = stateMachine.customer;
        
#if UNITY_EDITOR
        DebugUtil.AssertNullException(_customer,nameof(_customer));
#endif
        
        _animator = _customer.GetAnimator();
        _agent =_customer.GetNavMeshAgent();
        
#if UNITY_EDITOR
        DebugUtil.AssertNullException(_animator,nameof(_animator));
        DebugUtil.AssertNullException(_animator,nameof(_agent));
#endif
    }

    public virtual void Enter()
    {
#if UNITY_EDITOR
        Debug.Log(GetType().ToString());
#endif
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public void StartAnimation(int animationHash)
    {
        _animator.SetBool(animationHash,true);
    }

    public void StopAnimation(int animationHash)
    {
        _animator.SetBool(animationHash,false);
    }

    public void SetDestination(Vector3 pos)
    {
        _agent.SetDestination(pos);
    }
    
    
}