using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SliderPopup : UI_Popup
{
    public event Action<int> OnValueCallback;

    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;

    private int _value;
    private int _min = 1;
    private int _max;

    private SoundManager _soundManager;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        Debug.Assert(_slider, "Null Exception : _slider");
        Debug.Assert(_quantityText, "Null Exception : _quantityText");
        Debug.Assert(_plusButton, "Null Exception : _plusButton");
        Debug.Assert(_minusButton, "Null Exception : _minusButton");
#endif

        _slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        _plusButton.onClick.AddListener(() => OnButtonClicked(true));
        _minusButton.onClick.AddListener(() => OnButtonClicked(false));
    }

    private void Start()
    {
        if (_soundManager == null) _soundManager = SoundManager.Instance;
#if UNITY_EDITOR
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
#endif
    }

    public override void ShowPopup(PopupParameter popupParameter)
    {
        base.ShowPopup(popupParameter);

        SliderPopupParameter parameter = popupParameter as SliderPopupParameter;

        _max = parameter.GetSliderMaxValue();
        _slider.maxValue = _max;
        _value = (int)(_max * 0.5f);
        _slider.value = _value;
        _quantityText.text = $"{_value}";

        OnValueCallback = null;
        OnValueCallback += parameter.GetValueConfirmAction();
    }

    public void CallOnClicked()
    {
        OnValueCallback?.Invoke(_value);
    }

    private void OnButtonClicked(bool isPlus)
    {
        if (isPlus)
        {
            if (_value < _max) _value++;
        } else
        {
            if (_value > _min) _value--;
        }
        _quantityText.text = $"{_value}";
        _slider.value = _value;

        _soundManager.Play(Strings.Sounds.UI_BUTTON); 
    }

    private void OnSliderValueChanged()
    {
        if (_slider.value < 1) _slider.value = 1;
        _value = (int)(_slider.value);
        _quantityText.text = $"{_value}";
    }
}
