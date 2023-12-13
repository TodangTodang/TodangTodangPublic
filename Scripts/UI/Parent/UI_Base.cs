using DG.Tweening;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    public virtual void OpenUI(bool isSound = true, bool isAnimated = false)
    {
        gameObject.SetActive(true);
        UIManager.Instance.SetUIOnScreen(this, true);

        if (isSound)
            SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);

        if (isAnimated) UIEffect.AnimateUI(this, true);
    }

    public virtual void CloseUI(bool isSound = true, bool isAnimated = false)
    {
        UIManager.Instance.SetUIOnScreen(this, false);

        if (isSound)
            SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);

        if (isAnimated ) UIEffect.AnimateUI(this, false);
        else gameObject.SetActive(false);
    }
}
