using UnityEngine;

[CreateAssetMenu]
public class CookBookSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private IngredientInfoSO result;
    [SerializeField] private CookStepSO[] cookSteps;
    [SerializeField] private Sprite[] kitchenUtensils;
    [SerializeField] private Enums.CookStepInteraction[] firstInteraction;
    [SerializeField] private Enums.CookStepInteraction[] secondInteraction;

    public int ID => id;
    public IngredientInfoSO Result => result;
    public CookStepSO[] CookSteps => cookSteps;
    public Sprite[] KitchenUtensils => kitchenUtensils;

    public Enums.CookStepInteraction[] FirstInteraction => firstInteraction;
    public Enums.CookStepInteraction[] SecondInteraction => secondInteraction;
}
