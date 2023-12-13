using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialSkip : MonoBehaviour
{
    [SerializeField] private Button _skipButton;
    [SerializeField] private TutorialController _controller;

    private UIManager _uiMananger;

    private void Awake()
    {
        Debug.Assert(_skipButton != null, "Null Exception : _skipButton");
        _skipButton.onClick.AddListener(SkipTutorial);

        if (PlayerPrefs.HasKey(Strings.Prefs.IS_FIRST_TUTORIAL) 
            && PlayerPrefs.GetInt(Strings.Prefs.IS_FIRST_TUTORIAL) == 0)
        {
            _skipButton.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        if (_uiMananger == null) _uiMananger = UIManager.Instance;
        Debug.Assert(_uiMananger != null, "Null Exception : UIManager");
    }

    private void SkipTutorial()
    {
        _uiMananger.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.Tutorial.TUTORIAL_SKIP_POPUP,
                confirmCallback: () => { 
                        _controller.SkipTutorial();
                        PlayerPrefs.SetInt(Strings.Prefs.IS_TUTORIAL_SKIP, 1);
                    }
                )
            );
    }
}
