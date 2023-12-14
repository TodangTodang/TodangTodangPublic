using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class UIEffect
{
    private static Vector3 punchRotate = new Vector3(0, 0, 10);
    private static Dictionary<TMP_Text, bool> onGoingTextEffects = new Dictionary<TMP_Text, bool>();
    private static Dictionary<UI_Base, Tween> onGoingUIEffects = new Dictionary<UI_Base, Tween>();

    public static void EmphasizeText(TMP_Text text, int startValue, int endValue, Color startColor, Color endColor)
    {
        if (startValue == endValue) return;

        if (!FindEffect(text))
        {
            onGoingTextEffects[text] = true;
            text.rectTransform.DOPunchRotation(punchRotate, 1f, 2, 0.5f).OnComplete(() =>
            {
                onGoingTextEffects[text] = false;
            });
        }

        text.DOCounter(startValue, endValue, 0.5f, true);
        text.DOColor(endColor, 0.4f).SetEase(Ease.InOutBounce).OnComplete(() =>
        text.DOColor(startColor, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack));
        text.rectTransform.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.InOutBounce).OnComplete(() =>
        text.rectTransform.DOScale(Vector3.one, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack));
    }

    public static void AnimateUI(UI_Base ui, bool isOpen)
    {
        if (ui.TryGetComponent<CanvasGroup>(out CanvasGroup group))
        {
            float startFade = isOpen ? 0f : 1f;
            float endFade = isOpen ? 1f : 0f;
            Vector3 startScale = isOpen ? Vector3.one * 0.5f : Vector3.one;
            Vector3 endScale = isOpen ? Vector3.one : Vector3.one * 0.5f;

            ui.transform.GetChild(ui.transform.childCount - 1).TryGetComponent<RectTransform>(out RectTransform rect);
            group.alpha = startFade;
            rect.localScale = startScale;

            Sequence uiSequence = DOTween.Sequence();

            uiSequence.Append(rect.DOScale(endScale, 0.2f));
            uiSequence.Join(group.DOFade(endFade, 0.2f));
            uiSequence.OnComplete(() =>
            {
                if (!isOpen) ui.gameObject.SetActive(false);
            });

            if (onGoingUIEffects.ContainsKey(ui)) onGoingUIEffects[ui].Kill();
            onGoingUIEffects[ui] = uiSequence;
            
            uiSequence.SetUpdate(true).Play();
        }
    }

    private static bool FindEffect(TMP_Text text)
    {
        if (!onGoingTextEffects.ContainsKey(text)) onGoingTextEffects.Add(text, false);

        return onGoingTextEffects[text];
    }

    public static void ClearEffects()
    {
        onGoingTextEffects.Clear();

        foreach(KeyValuePair<UI_Base, Tween> kvp in onGoingUIEffects)
        {
            kvp.Value.Kill();
        }

        onGoingUIEffects.Clear();  
    }
}
