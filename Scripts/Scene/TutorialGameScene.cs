using UnityEngine;

public class TutorialGameScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.TutorialGameScene;

        SoundManager.Instance.Play(Strings.Sounds.BGM_GAMESCENE, Enums.AudioType.Bgm);
        return true;
    }

    public override void Clear()
    {
        Time.timeScale = 1f;
        PoolManager.Instance.Clear();
        base.Clear();
    }
}
