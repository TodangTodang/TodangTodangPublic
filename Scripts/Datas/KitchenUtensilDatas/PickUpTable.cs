using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTable : KitchenInteraction
{
    private List<IngredientInfoSO> orderedMenu;
    private Customer customer;
    private List<IngredientInfoSO> submitedMenu;
    
    private bool isCanPutDown = false;
    private void Awake()
    {
        ingredients = new List<GameObject>();
        submitedMenu = new List<IngredientInfoSO>();
        IsPlaceable = true;
    }

    private void RejectOrder()
    {
        customer.StopWaiting(CustomerEmotionType.Angry);
        customer.GiveFoods(false);
        ResetTable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (customer == null && other.TryGetComponent<Customer>(out Customer tmpCustomer))
        {
            customer = tmpCustomer;
            customer.Arrived(CheckFood,RejectOrder);
            orderedMenu = customer.OrderingMenu;
            isCanPutDown = true;
        }
    }

    public override void PutDown()
    {
        if (orderedMenu == null || !isCanPutDown) return;

        base.PutDown();
        
        if (ingredients.Count == orderedMenu.Count || !isCorrectFood())
        {
            StartCoroutine(CheckFood());
        }
    }

    private bool isCorrectFood()
    {
        IngredientInfoSO ingredientInfoSo = GetIngredient(ingredients[0]);
        bool isCorrect = orderedMenu.Contains(ingredientInfoSo);
        if (isCorrect)
        {
            customer.GetCorrectFoodMakeHappy();
        }

        return isCorrect;
    }

    public override void PickUp()
    {
        
    }

    IEnumerator CheckFood()
    {
        bool isRight;
        CustomerEmotionType currentCustomerEmotionType;
        isCanPutDown = false;
        
        foreach (GameObject ingredient in ingredients)
        {
            IngredientInfoSO ingredientInfoSO = GetIngredient(ingredient);
            submitedMenu.Add(ingredientInfoSO);
        }
 
        isRight = customer.IsCorrectMenu(submitedMenu, out currentCustomerEmotionType);
        if (!isRight)
        {
            customer.StopWaiting(CustomerEmotionType.Angry);    
        }
        else
        {
            customer.StopWaiting(currentCustomerEmotionType);
        }
        
        yield return CoroutineTime.GetWaitForSeconds(1f);
        
        customer.GiveFoods(isRight);
        ResetTable();
    }

    private void ResetTable()
    {
        foreach (var ingredient in ingredients)
        {
            ResourceManager.Instance.Destroy(ingredient.gameObject);
        }
        ingredients.Clear();
        submitedMenu.Clear();
        isCanPutDown = true;
        customer = null;
        orderedMenu = null;
    }
}
