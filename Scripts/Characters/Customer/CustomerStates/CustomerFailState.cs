
public class CustomerFailState : CustomerBaseState
{
    public CustomerFailState(CustomerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        SoundManager soundManager = SoundManager.Instance;
        
#if UNITY_EDITOR
        DebugUtil.AssertNullException(soundManager,nameof(soundManager));
#endif
        
        _customer.SetFace(Enums.FaceType.Angry);
        _customer.PlayEmotionEffect(false,CustomerEmotionType.Angry);
        _customer.CallOnDeliverMenu(CustomerEmotionType.Angry);
        _customer.InvokeExit();
        
        soundManager.Play(Strings.Sounds.CUSTOMER_FAIL);
    }
}
