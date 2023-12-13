
using System;
using UnityEngine;

[Serializable]
public class CustomerAnimationData
{
    [SerializeField] private string _moveParameterName = Strings.AnimationData.MOVE_PARAMETER;
    [SerializeField] private string _waitParameterName = Strings.AnimationData.WAIT_PARAMETER;

    public int moveParameterHash { get; private set; }
    public int waitParameterHash { get; private set; }
    
    public void Initialize()
    {
        moveParameterHash = Animator.StringToHash(_moveParameterName);
        waitParameterHash = Animator.StringToHash(_waitParameterName);
    }

}