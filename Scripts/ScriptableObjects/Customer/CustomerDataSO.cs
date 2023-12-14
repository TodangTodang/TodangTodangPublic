using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Customer/CustomerData", fileName = "CustomerData")]
public class CustomerDataSO : ScriptableObject
{
    [SerializeField] private float _baseEndurance;
    [SerializeField] private float[] _baseTipPayRate;
    [SerializeField] private Sprite[] _emotionText;
    public float BaseEndurance => _baseEndurance;
    public float[] BaseTipPayRate => _baseTipPayRate;
    public Sprite[] EmotionText => _emotionText;
}
