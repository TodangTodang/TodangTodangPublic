
using System.Collections;
using UnityEngine;

public abstract class FadeComponent : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    [SerializeField] private float fadeTransitionTime;
    [SerializeField] private bool onlyFadeOut;
    public virtual void StartFade()
    {
        StartCoroutine(StartFadeEffect(0f, 1f));
    }
    private IEnumerator StartFadeEffect(float start, float end)
    {
        SaveDefaultData();
        yield return FadeOperator.FadeLinear(end, start,fadeTime,OperationFade);
        yield return CoroutineTime.GetWaitForSeconds(fadeTransitionTime);
        if(!onlyFadeOut)
            yield return FadeOperator.FadeLinear(start, end,fadeTime,OperationFade);

        EndCallBack();
    }

    protected abstract void SaveDefaultData();

    protected abstract void EndCallBack();

    protected abstract void OperationFade(float rate);
}
