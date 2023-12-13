using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarrotAnimation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _range;

    void Start()
    {
        Animate();
    }

    void Animate()
    {
        // DoTween을 사용하여 localPosition.y 값을 -5부터 5까지 왔다갔다하도록 설정
        transform.DOLocalMoveY(_range, _duration) // 1.5초 동안 y를 5로 이동
            .SetEase(Ease.InOutQuad) // 이동에 Ease 효과 적용
            .SetLoops(-1, LoopType.Yoyo); // 무한 반복 및 Yoyo 효과 적용
    }
}
