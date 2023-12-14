using UnityEngine;
using UnityEngine.Serialization;

public class CustomerEnduranceController : MonoBehaviour
{
    [FormerlySerializedAs("progressImage")] [SerializeField] private UI_ProgressUI uiProgressImage;

    public void CheckProgressImage()
    {
#if UNITY_EDITOR
        DebugUtil.AssertNotAllocateInInspector(uiProgressImage!=null,nameof(uiProgressImage));
#endif
    }
    public void SetProgress(float value)
    {
        CheckProgressImage();
        uiProgressImage.SetProgressRate(value);
    }

    public void SetActiveEnduranceUI(bool isActive,CustomerEmotionType type)
    {
        CheckProgressImage();
        uiProgressImage.ActiveUI(isActive,type);
    }

    public CustomerEmotionType GetCustomerEmotionType()
    {
        CheckProgressImage();
        return uiProgressImage.GetCurrentEmotion();
    }

    public Sprite GetCustomerEmotionSprite(CustomerEmotionType type)
    {
        CheckProgressImage();
        return uiProgressImage.GetSprite(type);
    }
    
}
