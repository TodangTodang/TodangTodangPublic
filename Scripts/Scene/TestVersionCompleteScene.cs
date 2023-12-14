public class TestVersionCompleteScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.TestVersionCompleteScene;

        SoundManager.Instance.Play(Strings.Sounds.BGM_TESTVERSIONCOMPLETESCENE, Enums.AudioType.Bgm);

        return true;
    }
}
