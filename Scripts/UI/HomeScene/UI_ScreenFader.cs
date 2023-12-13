using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenFader : UI_Base
{
    [SerializeField] private Image _fadeEffectImage;
    [SerializeField] private Image _fadeEffectImageBox;

    [SerializeField] private float fadeTime;
    [SerializeField] private float fadeTransitionTime;

    [SerializeField] private float MaxSize;
    

    public void StartFadeCircle()
    {
        StartCoroutine(StartFadeEffect(0f, 1f));
    }

    public void StartFadeBoxFadeOut(float fadeTime = 0, Action endCallback = null)
    {
        if (fadeTime == 0)
            this.fadeTime = 1;
        else
        {
            this.fadeTime = fadeTime;
        }
        if(endCallback != null)
            StartCoroutine(StartBoxFadeWithAlpha(1f, 0f,endCallback));
    }

    private void alphaChange(float rate)
    {
        Color color = _fadeEffectImageBox.color;
        color.a = rate;
        _fadeEffectImageBox.color = color;
    }

    private void sizeChange(float rate)
    {
        _fadeEffectImage.gameObject.transform.localScale = Vector3.one * rate * MaxSize;
    }

    private void ChangeWidthHeight(float rate)
    {
        Vector2 rect = Vector2.one * rate * MaxSize;
        _fadeEffectImage.rectTransform.sizeDelta = rect;
    }

    private IEnumerator StartFadeEffect(float start, float end)
    {
        _fadeEffectImage.gameObject.SetActive(true);
        SoundManager.Instance.Play(Strings.Sounds.DAY_END);
        yield return FadeOperator.FadeLinear(end, start,fadeTime,ChangeWidthHeight);

        yield return CoroutineTime.GetWaitForSeconds(fadeTransitionTime);
        
        SoundManager.Instance.Play(Strings.Sounds.DAY_START);
        
        yield return FadeOperator.FadeLinear(start, end,fadeTime,ChangeWidthHeight);
        _fadeEffectImage.gameObject.SetActive(false);
        CloseUI(false);
    }

    private IEnumerator StartBoxFadeWithAlpha(float start, float end, Action endCallBack = null)
    {
        _fadeEffectImageBox.gameObject.SetActive(true);
        yield return FadeOperator.FadeLinear(end, start, fadeTime, alphaChange);
        endCallBack?.Invoke();
    }

    private void OnDisable()
    {
        UIManager.Instance.RemoveUIComponent<UI_ScreenFader>();
    }
}
