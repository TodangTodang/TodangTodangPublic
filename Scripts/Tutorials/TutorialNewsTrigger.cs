using UnityEngine;

public class TutorialNewsTrigger : MonoBehaviour
{
    [SerializeField] private TutorialOnClick _onClickListener;

    private void Start()
    {
        NewsSystem newsSystem = GameManager.Instance.GetNewsSystem();
        newsSystem.CheckTodaysNews(1,SetNextTutorialAction);
        newsSystem.ViewNews();
    }

    private void SetNextTutorialAction()
    {
        _onClickListener.IsClicked = true;
    }
}
