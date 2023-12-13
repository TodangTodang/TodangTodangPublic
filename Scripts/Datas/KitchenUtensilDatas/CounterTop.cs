public class CounterTop : KitchenInteraction
{
    protected override void Initialize()
    {
        GetUtensilData();
        CanInteractWithPlayer = true;
        IsPlaceable = true;
        interactionPos = foodPos[0];

        interactionSound = Strings.Sounds.KITCHEN_COUNTERTOP;

        base.Initialize();
    }
}
