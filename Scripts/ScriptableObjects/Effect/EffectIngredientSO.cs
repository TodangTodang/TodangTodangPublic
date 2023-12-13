using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effect/EffectIngredientSO", fileName = "EffectIngredientSO")]
public class EffectIngredientSO : EffectSO
{
    [field: SerializeField] public IngredientInfoSO Food { get; private set; }
    [field: SerializeField] public IngredientEffectType EffectType { get; private set; }
}