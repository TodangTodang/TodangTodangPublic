public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();  //�Է°� �޾ƿ���
    public void Update();

    public void FixedUpdate();
}