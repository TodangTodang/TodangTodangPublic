using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialDay1Game : MonoBehaviour
{
    [SerializeField] private Button _riceFlourSlotButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private TutorialOnClick[] _onClickListeners;
    [SerializeField] private Image _focusEffectImage;
    [SerializeField] private Transform _steamerPos;

    private int _clickIdx = 0;

    private UIManager _uiManager;
    private UI_IngredientBox _uiIngredientBox;

    private void Awake()
    {
        _riceFlourSlotButton.onClick.AddListener(SelectRiceFlour);
        _selectButton.onClick.AddListener(PickRiceFlour);
    }

    private void Update()
    {
        if (_focusEffectImage.gameObject.activeSelf)
        {
            _focusEffectImage.transform.position = Camera.main.WorldToScreenPoint(_steamerPos.transform.position + new Vector3(0f, 3f, 0f));
        }
    }

    private void SelectRiceFlour()
    {
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_uiIngredientBox == null) _uiIngredientBox = _uiManager.GetUIComponent<UI_IngredientBox>();
        UI_IngredientBoxSlot slot = _uiIngredientBox.GetSlot(0);
        _uiIngredientBox.OnSelectionChange(slot);
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);
        SetNextTutorialAction();
    }

    private void PickRiceFlour()
    {
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_uiIngredientBox == null) _uiIngredientBox = _uiManager.GetUIComponent<UI_IngredientBox>();
        _uiIngredientBox.SelectItem();
        SoundManager.Instance.Play(Strings.Sounds.UI_BUTTON);
        SetNextTutorialAction();
    }

    public void OpenSettingPanel()
    {
        if (_uiManager == null) _uiManager = UIManager.Instance;
        _uiManager.GetUIComponent<UI_PausePanel>().OpenUI();
    }

    private void SetNextTutorialAction()
    {
        if (_clickIdx < _onClickListeners.Length)
        {
            _onClickListeners[_clickIdx++].IsClicked = true;
        }
    }
}
