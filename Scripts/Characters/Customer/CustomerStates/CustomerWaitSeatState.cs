
using UnityEngine;

public class CustomerWaitSeatState : CustomerBaseState
{
    public CustomerWaitSeatState(CustomerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager soundManager = SoundManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(soundManager,nameof(soundManager));
#endif
        
        _agent.isStopped = true;
        _agent.angularSpeed = 0;
        _customer.gameObject.transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        _customer.OpenWaitUI();
        _customer.CallStartWaitOperation();
        StartAnimation(_customer.AnimationData.waitParameterHash);
        
        soundManager.Play(Strings.Sounds.CUSTOMER_ORDER);
    }

    public override void Exit()
    {
        StopAnimation(_customer.AnimationData.waitParameterHash);
        _customer.CloseWaitUI();
    }
}