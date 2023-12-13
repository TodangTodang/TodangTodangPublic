
public class CustomerStateMachine
{
    public CustomerEnterState EnterState;
    public CustomerExitState ExitState;
    public CustomerSuccessState SuccessState;
    public CustomerFailState FailState;
    public CustomerWaitSeatState WaitSeatState;
    public Customer customer;

    private ICustomerState currentState;
    public CustomerStateMachine(Customer customer)
    {
        this.customer = customer;

        EnterState = new CustomerEnterState(this);
        ExitState = new CustomerExitState(this);
        WaitSeatState = new CustomerWaitSeatState(this);
        SuccessState = new CustomerSuccessState(this);
        FailState = new CustomerFailState(this);
    }

    public ICustomerState GetCurrentState()
    {
        return currentState;
    }
    public void Update()
    {
        currentState.Update();
    }

    public void Init()
    {
        ChangeState(EnterState);
    }

    public void ChangeState(ICustomerState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}
