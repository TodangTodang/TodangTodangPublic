using UnityEngine;
using UnityEngine.UI;

public enum CustomerEmotionType
{
    Perfect, Great,SoSo,Angry
}
public class UI_ProgressUI : MonoBehaviour
{
    [SerializeField] private Slider ProgressSlider;
    [SerializeField] private Image handle;
    [SerializeField] private Sprite[] emotion;
    [SerializeField] private float emotionHoldTime;
    public void SetProgressRate(float rate)
    {
        ProgressSlider.value = rate;
        ChangeImage(rate);
    }

    private void ChangeImage(float rate)
    {
        if (rate >= 1)
        {
            if(handle.sprite != emotion[(int)CustomerEmotionType.Angry])
                handle.sprite = emotion[(int)CustomerEmotionType.Angry]; 
        }else if (rate >= 0.7f)
        {
            if(handle.sprite != emotion[(int)CustomerEmotionType.SoSo])
                handle.sprite = emotion[(int)CustomerEmotionType.SoSo];
        }else if (rate >= 0.4f)
        {
            if(handle.sprite != emotion[(int)CustomerEmotionType.Great])
                handle.sprite = emotion[(int)CustomerEmotionType.Great];
        }else if (rate >= 0)
        {
            if(handle.sprite != emotion[(int)CustomerEmotionType.Perfect])
                handle.sprite = emotion[(int)CustomerEmotionType.Perfect];
        }
    }

    public CustomerEmotionType GetCurrentEmotion()
    {
        float rate = ProgressSlider.value;
        if (rate >= 1f)
        {
            return CustomerEmotionType.Angry;
        }else if (rate >= 0.7f)
        {
            return CustomerEmotionType.SoSo;
        }else if (rate >= 0.4f)
        {
            return CustomerEmotionType.Great;
        }

        return CustomerEmotionType.Perfect;
    }

    public void ActiveUI(bool isActive,CustomerEmotionType type)
    {
        // if (isActive)
        // { 
        //     gameObject.SetActive(true);
        // }
        // else
        // {
        //     ProgressSlider.gameObject.SetActive(false);
        //     handle.sprite = emotion[(int)type];
        //     //Invoke(nameof(ActiveFalse),emotionHoldTime);
        // }
        
        gameObject.SetActive(isActive);
    }

    private void ActiveFalse()
    {
        ProgressSlider.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public Sprite GetSprite(CustomerEmotionType type)
    {
        return emotion[(int)type];
    }
}
