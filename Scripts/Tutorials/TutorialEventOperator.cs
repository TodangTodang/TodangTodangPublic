using UnityEngine;

public enum TutorialEventType
{
    GenerateTutorialCusotmer,
    InteractWithIngredientBox,
    CheckSteamerState,
    CheckFoodType,
    CheckPickUpTableState,
    ActiveAutoCustomerGenerate,
    UpdatePlayerMoney,
}

public abstract class TutorialEventOperator : MonoBehaviour
{
    public abstract bool SetNextEvent(TutorialEventType eventType);
}
