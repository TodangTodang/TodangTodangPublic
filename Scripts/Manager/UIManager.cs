using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // Resources/Prefabs/UI 안의 UI 컴포넌트 프리팹들을 저장하는 딕셔너리
    private readonly Dictionary<string, UI_Base> _uiDic = new Dictionary<string, UI_Base>();
    private readonly Dictionary<string, UI_Popup> _uiPopupDic = new Dictionary<string, UI_Popup>();

    private Stack<UI_Base> _uiOnScreen = new Stack<UI_Base>();

    public T GetUIComponent<T>() where T : UI_Base
    {
        string key = typeof(T).Name;
        if (!_uiDic.ContainsKey(key))
        {
            // 요청 시점에 해당 UI 컴포넌트가 없다면 Load
            GameObject prefab = Resources.Load<GameObject>($"{Strings.Prefabs.UI_PREFAB_PATH}{key}");
            if (!prefab)
            {
#if UNITY_EDITOR
                Debug.LogError($"UI Prefab 로드 실패 : {key}");
#endif
                return null;
            }
            GameObject obj = Instantiate(prefab);
            if (!obj.TryGetComponent<T>(out T component))
            {
#if UNITY_EDITOR
                Debug.LogError($"Get UI Component 실패 : {key}");
#endif
                return null;
            }
            _uiDic.Add(key, obj.GetComponent<T>());
        }
        return  _uiDic[key] as T;
    }

    public bool TryGetUIComponent<T>(out T uiComponent) where T : UI_Base
    {
        string key = typeof(T).Name;
        if (!_uiDic.ContainsKey(key))
        {
            // 요청 시점에 해당 UI 컴포넌트가 없다면 Load
            GameObject prefab = Resources.Load<GameObject>($"{Strings.Prefabs.UI_PREFAB_PATH}{key}");
            if (!prefab)
            {
#if UNITY_EDITOR
                Debug.LogError($"UI Prefab 로드 실패 : {key}");
#endif
                uiComponent = null;
                return false;
            }
            GameObject obj = Instantiate(prefab);
            if (!obj.TryGetComponent<T>(out T component))
            {
#if UNITY_EDITOR
                Debug.LogError($"Get UI Component 실패 : {key}");
#endif
                uiComponent = null;
                return false;
            }
            _uiDic.Add(key, component);
        }
        uiComponent = _uiDic[key] as T;
        return true;
    }

    public void RemoveUIComponent<T>() where T : UI_Base
    {
        string key = typeof(T).Name;
        if (_uiDic.ContainsKey(key))
        {
            _uiDic.Remove(key);
        }
    }

    public void RemoveAllUIComponent()
    {
        _uiDic.Clear();
    }

    public T ShowPopup<T>(PopupParameter popupParameter) where T : UI_Popup
    {
        string key = typeof(T).Name;
        if (!_uiPopupDic.ContainsKey(key))
        {
            // 요청 시점에 해당 UI 컴포넌트가 없다면 Load
            GameObject prefab = Resources.Load<GameObject>($"{Strings.Prefabs.UI_POPUP_PREFAB_PATH}{key}");
            if (!prefab)
            {
#if UNITY_EDITOR
                Debug.LogError($"UI Popup Prefab 로드 실패 : {key}");
#endif
                return null;
            }
            GameObject obj = Instantiate(prefab);
            if (!obj.TryGetComponent<T>(out T component))
            {
#if UNITY_EDITOR
                Debug.LogError($"Get UI Component 실패 : {key}");
#endif
                return null;
            }
            obj.transform.SetParent(transform);
            _uiPopupDic.Add(key, obj.GetComponent<T>());
        }

        _uiPopupDic[key].ShowPopup(popupParameter);

        return _uiPopupDic[key] as T;
    }

    public void SetUIOnScreen(UI_Base ui, bool isOpen)
    {
        if (isOpen)
        {
            if (ui is UI_Dialog) return;
            if (_uiOnScreen.Contains(ui)) return;

            _uiOnScreen.Push(ui);
        }
        else
        {
            if (!_uiOnScreen.Contains(ui)) return;

            while (_uiOnScreen.Peek() != ui)
            {
                _uiOnScreen.Pop();
            }

            _uiOnScreen.Pop();
        }
    }

    public UI_Base GetCurrentUI()
    {
        if (_uiOnScreen.TryPeek(out UI_Base ui)) return ui;
        return null;
    }

    public bool IsUIActivated<T>() where T : UI_Base
    {
        foreach (UI_Base ui in _uiOnScreen)
        {
            if (ui is T) return true;
            else continue;
        }

        return false;
    }

    public void CloseAllPopUps(bool isSound = false)
    {
        foreach (KeyValuePair<string, UI_Popup> popUp in _uiPopupDic)
        {
            if (popUp.Value.gameObject.activeSelf)
                popUp.Value.CloseUI();
        }
    }
}
