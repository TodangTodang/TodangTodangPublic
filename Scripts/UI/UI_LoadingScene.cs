using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScene : UI_Base
{
    [SerializeField] private Image _bunny;
    [SerializeField] private Image _moveArea;
    [SerializeField] private TMP_Text _tmiText;
    [SerializeField][TextArea] private string[] _tmi;

    private Animator _animator;
    private int _fallHash = Animator.StringToHash("End");

    private Scenes _nextSceneType = Scenes.Unknown;


    private void Start()
    {
        SoundManager.Instance.FadeBGM(false);

        _nextSceneType = SceneManagerEx.Instance.NextSceneType;
        _animator = _bunny.GetComponent<Animator>();
        _tmiText.text = _tmi[Random.Range(0, _tmi.Length)];
        Time.timeScale = 1;
        if (_nextSceneType != Scenes.Unknown) StartCoroutine(LoadSceneProcess());
        GameManager.Instance.ActivateEscapeInput(false);
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManagerEx.Instance.LoadSceneAsync(_nextSceneType);
        op.allowSceneActivation = false;

        float destination = _moveArea.rectTransform.rect.width;

        float timer = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;

            float newX = Mathf.Min(timer, 3f) / 3f * destination - (destination / 2);
            _bunny.rectTransform.anchoredPosition = new Vector2(newX, _bunny.rectTransform.anchoredPosition.y);

            if (timer > 3)
            {
                _animator.SetBool(_fallHash, true);
            }

            if (timer > 5f)
            {
                op.allowSceneActivation = true;
                SoundManager.Instance.FadeBGM(true);
                GameManager.Instance.ActivateEscapeInput(true);
            }

            yield return null;
        }
    }
}