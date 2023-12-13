using TMPro;
using UnityEngine;

public class UI_TextSetter : Poolable
{
    [SerializeField]private TMP_Text Text;

    public void Init(string moneyText)
    {
        Debug.Assert(this.Text,"this.moneyText가 inspector에서 설정되지 않았습니다");
        Text.text = moneyText;
    }
}
