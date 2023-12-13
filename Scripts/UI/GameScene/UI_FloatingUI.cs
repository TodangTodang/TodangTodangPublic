using DG.Tweening;
using UnityEngine;

public class UI_FloatingUI : MonoBehaviour
{
    [Range(1,10)][SerializeField] private float remainTime;

    [SerializeField] private CanvasGroup canvasGroup;
    private Vector3 currentVector;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(4, remainTime).SetEase(Ease.Linear));
        sequence.Join(canvasGroup.DOFade(0, remainTime+1));
        sequence.OnComplete(() =>
        {
            ResourceManager.Instance.Destroy(gameObject);
            canvasGroup.alpha = 1;
        });
    }
}
