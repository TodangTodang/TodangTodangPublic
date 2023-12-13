using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class PrologueScene : BaseScene
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private float WaitToPlay;
    [SerializeField] private SkipPrologue SkipPrologue;

    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.BankruptcyScene;

        Debug.Assert(_playableDirector, "_playableDirector를 등록하지 않았는데요?");
        Invoke(nameof(InvokingPlay), WaitToPlay);
        if (PlayerPrefs.HasKey(Strings.SaveData.IS_FIRST_PLAY))
        {
            int isFirstPlay = PlayerPrefs.GetInt(Strings.SaveData.IS_FIRST_PLAY);
            SkipPrologue.gameObject.SetActive(isFirstPlay == 1);
        }
        else
        {
            SkipPrologue.gameObject.SetActive(false);
        }

        SetStartVolume();

        return true;
    }

    private void InvokingPlay()
    {
        _playableDirector.Play();
    }

    private void SetStartVolume()
    {
        SoundManager _soundManager = SoundManager.Instance;

        _soundManager.Play("BGM/PrologueScene", Enums.AudioType.Bgm);
        _soundManager.FadeBGM(true);
    }

    public void EnableIsFirstPlay()
    {
        PlayerPrefs.SetInt(Strings.SaveData.IS_FIRST_PLAY,1);
    }
}
