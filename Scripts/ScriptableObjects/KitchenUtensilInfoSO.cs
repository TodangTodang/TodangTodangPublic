using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/KitchenUtensilInfoSO")]
public class KitchenUtensilInfoSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private int[] speedUpgradeInfo;
    [SerializeField] private int[] quantityUpgradeInfo;
    [SerializeField] private int[] upgradePrice;
    [SerializeField] private int[] requiredStarRating;

    public int ID => id;
    public string Name => name;
    public string Description => description;
    public Sprite IconSprite => iconSprite;
    public int[] SpeedUpgradeInfo => speedUpgradeInfo;
    public int[] QuantityUpgradeInfo => quantityUpgradeInfo;
    public int[] UpgradePrice => upgradePrice;
    public int[] RequiredStarRating => requiredStarRating;
}