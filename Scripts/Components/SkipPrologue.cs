using UnityEngine;
using UnityEngine.UI;

public class SkipPrologue : MonoBehaviour
{
    [SerializeField] private Button skipButton;

    public void Awake()
    {
        Debug.Assert(skipButton,$"skipButton {Strings.DebugLog.NOT_ALLOCATE_IN_INSPECTOR}");
        skipButton.onClick.AddListener(SkipOperation);
        
    }

    public void SkipOperation()
    {
        UIManager uiManager = UIManager.Instance;
        SceneManagerEx sceneManagerEx = SceneManagerEx.Instance;
        GameManager gameManager = GameManager.Instance;
        
        Debug.Assert(uiManager,$"uiManager {Strings.DebugLog.INIT_PROBLEM} ");
        Debug.Assert(sceneManagerEx,$"sceneManagerEx {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(gameManager,$"sceneManagerEx {Strings.DebugLog.INIT_PROBLEM}");
        
        uiManager.ShowPopup<UI_DefaultPopup>(new PopupParameter(
            content: "스킵하시겠습니까?",
            confirmCallback: () =>
            {
                GameManager.Instance.ChangeState(Enums.PlayerDayCycleState.StartStore);
                sceneManagerEx.LoadScene(Scenes.LoadingScene,Scenes.HomeScene);
            })
        );
    }
}
