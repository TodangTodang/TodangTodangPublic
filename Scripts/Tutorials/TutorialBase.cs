using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    // 튜토리얼 시작 시 호출
    public abstract void Enter();

    // 튜토리얼 진행 동안 매 프레임 호출
    public abstract void Execute(TutorialController controller);

    // 튜토리얼 종료 시 호출
    public abstract void Exit();
}
