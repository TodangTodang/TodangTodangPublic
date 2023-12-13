using System;
using UnityEngine;

public class StartScene : BaseScene
{
    private UIManager _uiManager;
    private SoundManager _soundManager;

    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.StartScene;

        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");

        //AnalyticsManager.Init();

        _soundManager.Play(Strings.Sounds.BGM_STARTSCENE, Enums.AudioType.Bgm);
        
        return true;
    }
}
