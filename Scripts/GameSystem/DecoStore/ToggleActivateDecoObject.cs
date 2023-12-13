using UnityEngine;

public class ToggleActivateDecoObject : IDecoObject
{
    private GameObject _changeObject;
    private GameObject _defaultObject; 

    public ToggleActivateDecoObject(GameObject changeObject, GameObject defaultObject)
    {
        _changeObject = changeObject;
        _defaultObject = defaultObject;
    }

    public void SetActivate(bool isActive)
    {
        if (_changeObject != null)
            _changeObject.SetActive(isActive);

        if (_defaultObject != null)
            _defaultObject.SetActive(!isActive);
    }
}
