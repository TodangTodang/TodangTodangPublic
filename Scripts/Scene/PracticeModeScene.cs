using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeModeScene : BaseScene
{
    [SerializeField] private Player _player;

    private UIManager _uiManager;
    private SoundManager _soundManager;

    private void Awake()
    {
        Debug.Assert(_player != null, "Null Exception : Player");
    }

    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.PracticeModeScene;

        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");

        if (_uiManager.TryGetUIComponent(out UI_PracticeModeScene uiPracticeModeScene))
            uiPracticeModeScene.SetPlayer(_player);
        _soundManager.Play(Strings.Sounds.BGM_GAMESCENE, Enums.AudioType.Bgm);
        return true;
    }
}
