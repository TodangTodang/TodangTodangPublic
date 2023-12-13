using System.Collections.Generic;
using UnityEngine;

public class UI_CookStepUpdater : MonoBehaviour
{
    private List<GameObject> slots = new List<GameObject>();
    private Dictionary<Enums.CookStepInteraction, string> _interactionDic = new Dictionary<Enums.CookStepInteraction, string>();
    private bool isInitialized = false;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (isInitialized) return;

        _interactionDic.Add(Enums.CookStepInteraction.Interaction, Strings.Prefabs.UI_INTERACTION);
        _interactionDic.Add(Enums.CookStepInteraction.MultipleInteraction, Strings.Prefabs.UI_MULTIPLE_INTERACTION);
        _interactionDic.Add(Enums.CookStepInteraction.PickUp, Strings.Prefabs.UI_PICK_UP);

        isInitialized = true;
    }

    public void UpdateCookStep(CookBookSO cookBookSO)
    {
        if (!isInitialized) Init();

        ResetSlots();
        UpdateData(cookBookSO);
    }

    private void UpdateData(CookBookSO cookbook)
    {
        for (int i = 0; i < cookbook.CookSteps.Length; i++)
        {
            GameObject cookStepSlot = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_COOKSTEP_SLOT, this.transform);
            cookStepSlot.transform.localScale = Vector3.one;

            slots.Add(cookStepSlot);

            int order = i + 1;
            Sprite utensil = cookbook.KitchenUtensils[i];
            List<IngredientInfoSO> ingredients = cookbook.CookSteps[i].Ingredients;
            IngredientInfoSO result = cookbook.CookSteps[i].ResultData;
            bool isLastIndex = (i == cookbook.CookSteps.Length - 1);

            string firstInteraction;
            string secondInteraction;

            _interactionDic.TryGetValue(cookbook.FirstInteraction[i], out firstInteraction);
            _interactionDic.TryGetValue(cookbook.SecondInteraction[i], out secondInteraction);

            UI_CookStepSlot component = cookStepSlot.GetComponent<UI_CookStepSlot>();
            component.InitCookStep(ingredients, result, isLastIndex);
            component.InitKitchenUtensil(order, utensil, firstInteraction, secondInteraction);
        }
    }

    private void ResetSlots()
    {
        foreach (GameObject obj in slots)
        {
            ResourceManager.Instance.Destroy(obj);
        }
        slots.Clear();
    }
}
