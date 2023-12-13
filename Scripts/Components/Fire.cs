using UnityEngine;

public class Fire : MonoBehaviour
{
    KitchenInteraction tryGetPot;
    KitchenInteraction interaction;

    private void OnTriggerEnter(Collider other)
    {
        GameObject pot = other.gameObject;
        if (pot.TryGetComponent<KitchenInteraction>(out tryGetPot))
        {
            if (!other.isTrigger) return;
            interaction = pot.GetComponent<KitchenInteraction>();
            interaction.Interaction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject pot = other.gameObject;
        if (pot.TryGetComponent<KitchenInteraction>(out tryGetPot))
        {
            interaction = null;
        }
    }
}
