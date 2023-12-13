using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effect/EffectCustomerSO", fileName = "EffectCustomerSO")]
public class EffectCustomerSO : EffectSO
{
    [field:SerializeField] public CustomerEffectType EffectType { get; /*private*/ set; }

}