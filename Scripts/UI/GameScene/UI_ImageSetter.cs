using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_ImageSetter : Poolable
{
    [FormerlySerializedAs("image")] [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    public void Init(Sprite newImage)
    {
        Debug.Assert(image1,"image가 inspector에서 설정되지 않았습니다");
        image1.sprite = newImage;
    }
    public void Init(Sprite newImage1, Sprite newImage2)
    {
        Init(newImage1);
        image2.sprite = newImage2;
    }
}
