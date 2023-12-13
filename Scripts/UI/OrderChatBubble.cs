using UnityEngine;
using UnityEngine.UI;

public class OrderChatBubble : MonoBehaviour
{
    [SerializeField] private Image foodImage;
    [SerializeField] private Image teaImage;
    [SerializeField] private GameObject orderChatBubbleObj;


    public void SetOrder(Sprite food, Sprite drink=null)
    {
        foodImage.sprite = food;
        if (drink)
        {
            teaImage.gameObject.SetActive(true);
            teaImage.sprite = drink;
        }
        else
        {
            teaImage.gameObject.SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        orderChatBubbleObj.SetActive(value);
    }


}
