public class CustomerSuccessState : CustomerBaseState
{
    public CustomerSuccessState(CustomerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CustomerEmotionType currentEmotionType = _customer.GetEmotionType();
        SoundManager soundManager = SoundManager.Instance;
        
#if UNITY_EDITOR
        DebugUtil.AssertNullException(soundManager,nameof(soundManager));
#endif
        _customer.SetFace(Enums.FaceType.Happy);
        _customer.PlayEmotionEffect(true,currentEmotionType);
        _customer.CallOnDeliverMenu(currentEmotionType);
        _customer.InvokeExit();
        
        soundManager.Play(Strings.Sounds.CUSTOMER_SUCCESSE);
        
    }
}
