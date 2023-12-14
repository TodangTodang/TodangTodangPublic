using System.Collections;
using UnityEngine;

public class NPCCat1 : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            _animator.SetTrigger("Spin");

            yield return CoroutineTime.GetWaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

            yield return CoroutineTime.GetWaitForSeconds(5f); 

            _animator.SetTrigger("Clap");
            yield return CoroutineTime.GetWaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length); 
        }
    }

}