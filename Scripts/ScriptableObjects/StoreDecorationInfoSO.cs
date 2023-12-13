using UnityEngine;

[CreateAssetMenu(fileName = "DecoStore", menuName = "DecoStore/StoreDecorationInfoSO")]
public class StoreDecorationInfoSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private EffectCustomerSO effect;

    public int ID => id;
    public string Name => name;
    public string Description => description;
    public int Price => price;
    public Sprite IconSprite => iconSprite;
    public EffectCustomerSO Effect => effect;
}