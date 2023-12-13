using UnityEngine;

public class LoadingScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.LoadingScene;
        UIManager.Instance.GetUIComponent<UI_LoadingScene>();
        Time.timeScale = 1f;
        return true;
    }

    public override void Clear()
    {
        base.Clear();
    }
}
