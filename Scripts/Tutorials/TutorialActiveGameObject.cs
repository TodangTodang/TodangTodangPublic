using UnityEngine;

public class TutorialActiveGameObject : TutorialBase
{
    [SerializeField] private GameObject[] _activeObjects;
    [SerializeField] private GameObject[] _inactiveObjects;

    public override void Enter()
    {
        for (int i = 0; i < _activeObjects.Length; i++) _activeObjects[i].SetActive(true);
        for (int i = 0; i < _inactiveObjects.Length; i++) _inactiveObjects[i].SetActive(false);
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
}
