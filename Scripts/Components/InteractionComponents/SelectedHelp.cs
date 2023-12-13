using UnityEngine;

public class SelectedHelp : MonoBehaviour, ISelection
{
    private UIManager _uiManager;

    private UI_PracticeHelp _practiceHelp;
    private UI_HelpButton _helpButton;

    [SerializeField] private string _name;
    private Sprite _sprite;

    private void Start()
    {
        _uiManager = UIManager.Instance;

        Debug.Assert(_uiManager != null, "NULL : UIManager");

        _practiceHelp = _uiManager.GetUIComponent<UI_PracticeHelp>();
        _helpButton = _uiManager.GetUIComponent<UI_HelpButton>();

        _practiceHelp.gameObject.SetActive(false);
        _helpButton.gameObject.SetActive(false);
        
        KitchenInteraction utensil = GetComponent<KitchenInteraction>();
        _sprite = ResourceManager.Instance.LoadSprite($"PracticeHelps/{utensil.name}");
    }

    public void SelectObject(bool isSelected)
    {
        if (isSelected)
        {
            _practiceHelp.Init(_sprite, _name);
            _helpButton.OpenUI();
            _helpButton.AddButtonAction(OpenPracticeHelp);
        }
        else
        {
            _helpButton.CloseUI();
            _helpButton.RemoveButtonAction();
        }
    }

    public void OpenPracticeHelp()
    {
        _practiceHelp.OpenUI();
    }
}
