using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialDay1Start : MonoBehaviour
{
    [SerializeField] private Button _startStoreButton;

    private SceneManagerEx _sceneManagerEx;
    private SoundManager _soundManager;

    private void Awake()
    {
        _startStoreButton.onClick.AddListener(LoadGameScene);
    }

    private void Start()
    {
        InitManagers();
    }

    private void InitManagers()
    {
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;

#if UNITY_EDITOR
        Debug.Assert(_sceneManagerEx != null, "Null Exception : SceneManagerEx");
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
#endif
    }

    private void LoadGameScene()
    {
        _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.TutorialGameScenePC);
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
    }
}
