
using System;
using UnityEngine;

[Serializable]
public class EffectSO : ScriptableObject
{
    [field:SerializeField] public float EffectRate { get; private set; }
}

public enum CustomerEffectType
{
    Endurance, CustomerCount, CustomerPayrate,
}

public enum IngredientEffectType
{
    Price,
}
