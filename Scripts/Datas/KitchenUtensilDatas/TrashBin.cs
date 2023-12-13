public class TrashBin : KitchenInteraction
{
    protected override void Initialize()
    {
        IsPlaceable = true;
    }

    public override void Interaction()
    {
        
    }

    public override void PickUp()
    {
        
    }

    public override void PutDown()
    {
        base.PutDown();
        ResourceManager.Instance.Destroy(ingredients[0]);
        workingParticle.Play();
        ingredients.Clear();
    }
}
