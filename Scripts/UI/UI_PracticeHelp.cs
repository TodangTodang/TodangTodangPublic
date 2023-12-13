using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PracticeHelp : UI_Base
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _description;
    [SerializeField] private Button _closeBtn;

    private PlayerInput _input;

    private void Awake()
    {
        _closeBtn.onClick.AddListener(()=> CloseUI());
    }

    public void Init(Sprite sprite, string name)
    {
        _name.text = name;
        _description.sprite = sprite;
    }

    public void SetPlayerInput(PlayerInput input)
    {
        _input = input;
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        _input.EnableInput(false);
        Time.timeScale = 0f;
        base.OpenUI(isSound, isAnimated);
    }

    public override void CloseUI(bool isSound = true, bool isAnimated = true)
    {
        _input.EnableInput(true);
        Time.timeScale = 1f;
        base.CloseUI(isSound, true);
    }
}
