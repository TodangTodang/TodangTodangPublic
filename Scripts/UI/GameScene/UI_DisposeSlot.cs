using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DisposeSlot : UI_Base
{
    [SerializeField] private Image _foodImage;
    [SerializeField] private TMP_Text _count;

    public void Init(Sprite foodImage, string Count)
    {
        _foodImage.sprite = foodImage;
        _count.text = Count;
    }
}
