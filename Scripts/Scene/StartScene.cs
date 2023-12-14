using System;
using UnityEditor.Rendering;
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

        SetResolution(); 

        _soundManager.Play(Strings.Sounds.BGM_STARTSCENE, Enums.AudioType.Bgm);
        
        return true;
    }

    private void SetResolution()
    {
        int saveScreen = PlayerPrefs.GetInt(Strings.Prefs.SAVESCREEN, -1);

        if(saveScreen == -1)
        {
            Resolution optimalResolution = GetOptimalResolution();
            Screen.SetResolution(optimalResolution.width, optimalResolution.height, Screen.fullScreenMode);
        }
    }

    private Resolution GetOptimalResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        Resolution optimalResolution = resolutions[0];

        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width > optimalResolution.width)
            {
                optimalResolution = resolution;
            }
        }

        return optimalResolution;
    }
}
