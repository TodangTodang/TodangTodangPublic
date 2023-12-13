using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestVersionComplete : MonoBehaviour
{
    [SerializeField] private Button _toStartButton;

    private void Start()
    {
        _toStartButton.onClick.AddListener(ToStartScene);
    }

    private void ToStartScene()
    {
        SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.StartScene);
    }
}
