using UnityEngine;

public class CustomerEnduranceUI : MonoBehaviour
{
    [SerializeField] private ProgressUI progressImage;

    public void CheckProgressImage()
    {
#if UNITY_EDITOR
        DebugUtil.AssertNotAllocateInInspector(progressImage!=null,nameof(progressImage));
#endif
    }
    public void SetProgress(float value)
    {
        CheckProgressImage();
        progressImage.SetProgressRate(value);
    }

    public void SetActiveEnduranceUI(bool isActive,CustomerEmotionType type)
    {
        CheckProgressImage();
        progressImage.ActiveUI(isActive,type);
    }

    public CustomerEmotionType GetCustomerEmotionType()
    {
        CheckProgressImage();
        return progressImage.GetCurrentEmotion();
    }

    public Sprite GetCustomerEmotionSprite(CustomerEmotionType type)
    {
        CheckProgressImage();
        return progressImage.GetSprite(type);
    }
    
}
