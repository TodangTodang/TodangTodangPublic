using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CookStepSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _orderTxt;
    [SerializeField] private Image _utensilIcon;
    [SerializeField] private Transform _ingredients;
    [SerializeField] private Image _resultIcon;
    [SerializeField] private TMP_Text _resultName;
    [SerializeField] private Image _resultBackground;
    [SerializeField] private Transform _firstInteraction;
    [SerializeField] private Transform _secondInteraction;

    private Color _normalColor;
    private Color _resultColor;

    private List<GameObject> _ingredientObjs;

    private GameObject firstInteractionObj;
    private GameObject secondInteractionObj;

    public void InitCookStep(List<IngredientInfoSO> ingredients, IngredientInfoSO result, bool isLastStep)
    {
        ColorUtility.TryParseHtmlString("#FFD9D9", out _normalColor);
        ColorUtility.TryParseHtmlString("#FFAFAF", out _resultColor);

        _ingredientObjs = new List<GameObject>();

        _resultIcon.sprite = result.IconSprite;
        _resultName.text = result.Name.ToString();

        _resultBackground.color = isLastStep ? _resultColor : _normalColor;

        foreach (var ingredient in ingredients)
        {
            CreateIngredientIcon(ingredient);
        }
    }

    public void InitKitchenUtensil(int order, Sprite utensil, string firstInteraction, string secondInteraction)
    {
        _orderTxt.text = order.ToString();
        _utensilIcon.sprite = utensil;

        if (firstInteraction != null)
        {
            firstInteractionObj = ResourceManager.Instance.Instantiate(firstInteraction, _firstInteraction);
            firstInteractionObj.transform.localPosition = Vector3.zero;
            firstInteractionObj.transform.localScale = Vector3.one;
        }
        
        if (secondInteraction != null)
        {
            secondInteractionObj = ResourceManager.Instance.Instantiate(secondInteraction, _secondInteraction);
            secondInteractionObj.transform.localPosition = Vector3.zero;
            secondInteractionObj.transform .localScale = Vector3.one;
        }
    }


    private void CreateIngredientIcon(IngredientInfoSO ingredient)
    {
        if (ingredient is IntermediateResultsInfoSO || ingredient is ResultInfoSO)
        {
            GameObject obj = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_INGREDIENT, _ingredients);
            obj.GetComponent<UI_Ingredient>().Init(ingredient);
            obj.transform.localScale = Vector3.one;
            _ingredientObjs.Add(obj);
        }
        else
        {
            GameObject obj = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_BASE_INGREDIENT, _ingredients);
            obj.GetComponent<UI_Ingredient>().Init(ingredient);
            obj.transform.localScale = Vector3.one;
            if (_ingredientObjs == null) Debug.Log("list null");
            _ingredientObjs.Add(obj);
        }
    }

    private void OnDisable()
    {
        foreach (var ingredient in _ingredientObjs)
        {
            ResourceManager.Instance.Destroy(ingredient);
        }

        if (firstInteractionObj != null)
        {
            ResourceManager.Instance.Destroy(firstInteractionObj);
            firstInteractionObj = null;
        }

        if (secondInteractionObj != null)
        {
            ResourceManager.Instance.Destroy(secondInteractionObj);
            secondInteractionObj = null;
        }
        _ingredientObjs.Clear();
    }
}
