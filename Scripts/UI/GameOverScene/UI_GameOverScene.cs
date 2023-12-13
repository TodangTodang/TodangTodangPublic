using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverScene : MonoBehaviour
{
    [SerializeField] private Image _rabbitImage;
    [SerializeField] private Sprite _rabbitSitSprite;
    [SerializeField] private GameObject _gameOverNews;
    [SerializeField] private Image _screenFader;
    [SerializeField] private Button _closeBtn;

    private void Awake()
    {
        _closeBtn.onClick.AddListener(LoadStartScene);
    }

    public void ChangeRabbitSprite()
    {
        _rabbitImage.sprite = _rabbitSitSprite;
    }

    public void ShowGameOverNews()
    {
        _gameOverNews.SetActive(true);
        StartCoroutine(ShowCloseBtn());
    }

    private IEnumerator ShowCloseBtn()
    {
        yield return CoroutineTime.GetWaitForSeconds(4f);
        _closeBtn.gameObject.SetActive(true);
    }

    private void LoadStartScene()
    {
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON, Enums.AudioType.Effect);
        
        GameManager gameManager = GameManager.Instance;
        Debug.Assert(gameManager, $"dataManager {Strings.DebugLog.INIT_PROBLEM}");
        PlayerData playerData = gameManager.GetPlayerData();
        playerData.UpdateEndingState(Enums.PlayerEndingState.ContinuePlaying);
        
        StartCoroutine(StartFadeEffect(0f, 1f));
    }

    private IEnumerator StartFadeEffect(float start, float end)
    {
        _screenFader.gameObject.SetActive(true);
        
        yield return FadeOperator.FadeLinear(start, end, 4f, AlphaChange);

        SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.StartScene);
    }

    private void AlphaChange(float rate)
    {
        Color color = _screenFader.color;
        color.a = rate;
        _screenFader.color = color;
    }

    #region Sounds
    public void PlayCricketSFX()
    {
        SoundManager.Instance.Play(Strings.Sounds.GAMEOVER_SFX_CRICKETS, Enums.AudioType.Effect);
    }

    public void PlayMurmurSFX()
    {
        SoundManager.Instance.Play(Strings.Sounds.GAMEOVER_SFX_MURMUR, Enums.AudioType.Effect);
    }

    public void PlayNewspapaerSFX()
    {
        SoundManager.Instance.Play(Strings.Sounds.GAMEOVER_SFX_NEWSPAPER, Enums.AudioType.Effect);
    }

    public void PlayRabbitFootstepSFX()
    {
        SoundManager.Instance.Play($"{Strings.Sounds.PLAYER_FOOTSTEP}1", Enums.AudioType.Effect);
    }

    public void PlayRabbitFallDownSFX()
    {
        SoundManager.Instance.Play(Strings.Sounds.PLAYER_FALLDOWN, Enums.AudioType.Effect);
    }
    #endregion
}
