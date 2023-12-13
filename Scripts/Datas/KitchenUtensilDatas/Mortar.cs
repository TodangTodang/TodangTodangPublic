public class Mortar : KitchenInteraction
{
    protected override void Initialize()
    {
        GetUtensilData();     
        CanInteractWithPlayer = true;
        IsPlaceable = true;
        interactionPos = foodPos[0];

        interactionSound = Strings.Sounds.KITCHEN_MORTAR;

        base.Initialize();
    }
}
