using UnityEngine;

public class ActivateDecoObject : IDecoObject
{
    private GameObject _object; 

    public ActivateDecoObject(GameObject obj)
    {
        _object = obj;
    }

    public void SetActivate(bool isActive)
    {
        if (_object != null)
        {
            _object.SetActive(isActive);
        }
    }
}
