public class BankruptcyScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.BankruptcyScene;

        SoundManager.Instance.Play("BGM/BankruptcyScene", Enums.AudioType.Bgm); 

        return true;
    }
}
