
using System;
using UnityEngine;

[Serializable]
public class CustomerData
{
    private CustomerDataSO dataSo;

    public float currentMaxEndurance { get; private set; }
    public float[] currentTipRates { get; private set; }
    public float currentPayRate { get; private set; }

    public Sprite[] emotionImageText => dataSo.EmotionText;

    //음식 리스트
    public void Init(CustomerDataSO data, float extraEnduranceRate, float extraPayRate)
    {
#if UNITY_EDITOR
        DebugUtil.AssertNullException(data,nameof(data));
#endif

        dataSo = data;
        currentMaxEndurance = dataSo.BaseEndurance * (1 + extraEnduranceRate);
        currentPayRate = 1 + extraPayRate;
        currentTipRates = dataSo.BaseTipPayRate;
    }
    
}
