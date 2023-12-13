public class Pot : KitchenInteraction
{

    protected override void Initialize()
    {
        GetUtensilData();

        CanInteractWithPlayer = false;
        IsPlaceable = true;

        interactionPos = foodPos[0];

        base.Initialize();

        successSound = Strings.Sounds.KITCHEN_WAIT_SUCCESS;
    }

    public override void  Interaction()  //불에 닿일 때 호출
    {
        if (ingredients.Count <= 0) return;
        if (ingredients[0].tag == "Trash") return;

        ++currentProgress;

        if (currentProgress != maxProgress) soundManager.Play(Strings.Sounds.KITCHEN_WARNING);
        
        UpdateProgressBar();

        if (currentProgress == maxProgress)
        {
            CheckValidity();
            return;
        }
        else if (currentProgress > maxProgress + 1)
        {
            if (currentProgress < maxProgress + 3)
            {
                UpdateWarning(1);
                soundManager.Play(Strings.Sounds.KITCHEN_WARNING_FAST);
            }
            else if (currentProgress < maxProgress + 4)
            {
                UpdateWarning(2);
                soundManager.Play(Strings.Sounds.KITCHEN_WARNING_FAST);
            }
            else if (currentProgress < maxProgress + 5)
            {
                MakeResult();
                currentProgress = 0;
                UpdateWarning(0);
            }
        }
    }
}
