using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomerRejectButton : MonoBehaviour
{
    [SerializeField]private Button rejectButton;

    public void AddListener(UnityAction action)
    {
        Debug.Assert(rejectButton, $"RejectButton {Strings.DebugLog.INIT_PROBLEM}");
        rejectButton.onClick.AddListener(action);
    }

    public void ActiveButton(bool isActive)
    {
        rejectButton.gameObject.SetActive(isActive);
    }
}
