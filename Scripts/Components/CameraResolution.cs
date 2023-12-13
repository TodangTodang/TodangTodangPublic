using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [SerializeField] private Transform _standardResolution;

    private Camera _camera;
    private int previousRatio; 

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        int currentRatio = Screen.width / Screen.height;

        if(!currentRatio.Equals(previousRatio))
        {
            previousRatio = currentRatio;

            if (IsObjectInScreen())
            {
                if (_camera.orthographic)
                    _camera.orthographicSize = Numbers.DEFAULT_ORTHOGRAPHICSIZE;
                else
                    _camera.fieldOfView = Numbers.DEFAULT_FIELDOFVIEW;
            }
            else
            {
                StartCoroutine(UpdateCameraSize()); 
            }
        }
    }

    private IEnumerator UpdateCameraSize()
    {
        while (!IsObjectInScreen())
        {
            if (_camera.orthographic)
                _camera.orthographicSize += 1;
            else
                _camera.fieldOfView += 1;

            yield return null; 
        }
    }

    private bool IsObjectInScreen()
    {
        Vector3 updatedScreenPoint = _camera.WorldToScreenPoint(_standardResolution.position);
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        return (updatedScreenPoint.x >= 0 && updatedScreenPoint.x <= screenWidth &&
                updatedScreenPoint.y >= 0 && updatedScreenPoint.y <= screenHeight);
    }
}
