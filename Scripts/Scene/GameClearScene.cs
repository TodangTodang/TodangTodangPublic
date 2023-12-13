using System;
using UnityEngine;

public class GameClearScene : BaseScene
{
    public void Start()
    {
        SoundManager soundManager = SoundManager.Instance;
        Debug.Assert(soundManager,$"soundManager {Strings.DebugLog.INIT_PROBLEM}");
        
        soundManager.Play(Strings.Sounds.BGM_GAMECLEARSCENE, Enums.AudioType.Bgm);
    }
}
