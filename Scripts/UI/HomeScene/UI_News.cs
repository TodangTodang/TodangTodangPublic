using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_News : UI_Base
{
    public event Action OnClosed;

    [SerializeField] private TMP_Text _day;
    [SerializeField][TextArea] private TMP_Text _headLine;
    [SerializeField] private TMP_Text _contents;
    [SerializeField] private Button _closeBtn;

    private void Start()
    {
        _closeBtn.onClick.AddListener(() => CloseUI());
        _closeBtn.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ShowCloseBtn());
    }

    public void ShowNews(NewsSO news, int day)
    {
        _day.text = $"Day {day}";
        _headLine.text = news.HeadLine;
        _contents.text = news.Contents;
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, true);
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        OnClosed?.Invoke();
        base.CloseUI(isSound, isAnimated);
    }

    IEnumerator ShowCloseBtn()
    {
        yield return CoroutineTime.GetWaitForSeconds(3f);
        _closeBtn.gameObject.SetActive(true);
    }
}
