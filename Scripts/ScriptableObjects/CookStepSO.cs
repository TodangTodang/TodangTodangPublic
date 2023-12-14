using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CookStepSO : ScriptableObject
{
    [SerializeField] public List<IngredientInfoSO> Ingredients;
    [SerializeField] public GameObject result; 

    public IngredientInfoSO ResultData
    {
        get
        {
            IngredientObject ingredientObj;
            IngredientInfoSO ingredientInfoSo;
            result.TryGetComponent<IngredientObject>(out ingredientObj);
            ingredientInfoSo = ingredientObj.GetIngredientObjectSO();
            return ingredientInfoSo;
        }
    }
}
