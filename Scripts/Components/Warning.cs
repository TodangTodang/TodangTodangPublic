using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    [SerializeField] private Image warning;
    private Coroutine blinkingCoroutine;
    private float blinkDuration;
    private float normalBlink = 0.3f;
    private float fastBlink = 0.1f;

    private void OnEnable()
    {
        blinkDuration = normalBlink;
        blinkingCoroutine = StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {

            float elapsedTime = 0f;
            float currentDuration = GetCurrentDuration();

            while (elapsedTime < currentDuration)
            {
                warning.color = new Color(warning.color.r, warning.color.g, warning.color.b, Mathf.Lerp(1f, 0f, elapsedTime / currentDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return CoroutineTime.GetWaitForSeconds(currentDuration);

            elapsedTime = 0f;

            while (elapsedTime < currentDuration)
            {
                warning.color = new Color(warning.color.r, warning.color.g, warning.color.b, Mathf.Lerp(0f, 1f, elapsedTime / currentDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return CoroutineTime.GetWaitForSeconds(currentDuration);
        }
    }

    public void SetBlinkTerm(int stage)
    {
        switch (stage)
        {
            case 1:
                blinkDuration = normalBlink;
                break;
            case 2:
                blinkDuration = fastBlink;
                break;
            default:
                blinkDuration = normalBlink;
                break;
        }
    }

    private float GetCurrentDuration()
    {
        return blinkDuration;
    }

    private void OnDisable()
    {
        StopCoroutine(blinkingCoroutine);
    }
}
