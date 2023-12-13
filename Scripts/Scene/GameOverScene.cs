public class GameOverScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.GameOverScene;

        SoundManager.Instance.Play(Strings.Sounds.BGM_GAMEOVERSCENE, Enums.AudioType.Bgm);

        return true;
    }

    public override void Clear()
    {
        base.Clear();
        DataManager.Instance.DeletePlaySaveDataAll();
    }
}
