using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialBase> _tutorials;
    [SerializeField] private List<TutorialBase> _tutorialSkip;

    public event Action OnTutorialEnd;

    private TutorialBase _currentTutorial = null;
    private int _currentIdx = -1;

    private GameManager _gameManager;
    private UIManager _uiManager;

    private void Start()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        SetNextTutorial();
    }

    private void Update()
    {
        if (_currentTutorial != null)
        {
            _currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (_currentIdx == 0)
        {
            _gameManager.SetControlMode(true);
        }

        // 현재 튜토리얼 Exit
        if (_currentTutorial != null) _currentTutorial.Exit();

        // 마지막 튜토리얼이라면 튜토리얼 종료
        if (_currentIdx >= _tutorials.Count - 1)
        {
            CompleteAllTutorials();
            return;
        }

        // 다음 튜토리얼 등록
        _currentIdx++;
        _currentTutorial = _tutorials[_currentIdx];

        // 새 튜토리얼 Enter
        _currentTutorial.Enter();
    }

    public void CompleteAllTutorials()
    {
        _currentTutorial = null;
        OnTutorialEnd?.Invoke();
        _gameManager.SetControlMode(false);
    }

    public void SkipTutorial()
    {
        CompleteAllTutorials();
        _tutorials = _tutorialSkip;
        _currentIdx = -1;
        if (!_uiManager.TryGetUIComponent(out UI_Dialog uiDialog))
        {
            Debug.LogError("Null Exception : UI_Dialog");
            return;
        }
        uiDialog.CloseUI(false);
        SetNextTutorial();
    }
}
