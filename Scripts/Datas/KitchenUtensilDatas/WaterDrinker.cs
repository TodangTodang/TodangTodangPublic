using UnityEngine;

public class WaterDrinker : KitchenInteraction
{
    protected override void Initialize()
    {
        base.Initialize();
        CanInteractWithPlayer = true;

        interactionSound = "";
        successSound = Strings.Sounds.KITCHEN_WATER;
    }

    public override void Interaction()
    {
        if (player.Ingredient == null) return;
        if (player.Ingredient.tag == "Trash") return;
        if (player.Ingredient != null)
        {
            ingredients.Add(player.Ingredient);

        }
        player.Ingredient = null;
        interactionPos = player.foodPos;

        base.Interaction();
    }

    protected override void MakeResult(GameObject result)
    {
        base.MakeResult(result);
        currentProgress = 0;
    }

}
