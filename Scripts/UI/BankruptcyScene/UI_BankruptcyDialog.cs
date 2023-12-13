using System;
using System.Collections;
using UnityEngine;

public class UI_BankruptcyDialog : UI_Base
{
    [SerializeField] DialogSystem _dialogSystem;
    public event Action FinishedDialogs;

    public void Start()
    {
        _dialogSystem.Init();
        StartCoroutine(ShowDialogs()); 
    }

    private IEnumerator ShowDialogs()
    {
        while (!_dialogSystem.UpdateDialog())
        {
            yield return null;
        }

        if(SceneManagerEx.Instance.CurrentSceneType != Scenes.HomeScene)
        {
            FinishedDialogs.Invoke();
        }
       
    }
}
