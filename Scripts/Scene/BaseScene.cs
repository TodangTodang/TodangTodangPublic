using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    public Scenes SceneType = Scenes.Unknown;

    protected bool _init = false;

    private void Start()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_init) return false;

        _init = true;
        Object go = FindObjectOfType(typeof(EventSystem));
        if (go == null) Instantiate(Resources.Load("Prefabs/UI/EventSystem"));

        return true;
    }

    public virtual void Clear() 
    {
        UIManager.Instance.CloseAllPopUps();
        UIManager.Instance.RemoveAllUIComponent();
    }
}
