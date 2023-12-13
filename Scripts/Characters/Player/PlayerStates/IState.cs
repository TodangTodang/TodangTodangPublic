public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();  //입력값 받아오기
    public void Update();

    public void FixedUpdate();
}