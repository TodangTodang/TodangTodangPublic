using UnityEngine;
using UnityEngine.UI;

public class UI_ImagePopup : UI_Popup
{
    [SerializeField] private Image _curIconImage;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    private int _curIdx;
    private Sprite[] _spriteList;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        Debug.Assert(_curIconImage, "Null Exception : _curIconImage");
        Debug.Assert(_prevButton, "Null Exception : _prevButton");
        Debug.Assert(_nextButton, "Null Exception : _nextButton");
#endif

        _prevButton.onClick.AddListener(ShowPrevImage);
        _nextButton.onClick.AddListener(ShowNextImage);
    }

    public override void ShowPopup(PopupParameter popupParameter)
    {
        base.ShowPopup(popupParameter);

        ImagePopupParameter parameter = popupParameter as ImagePopupParameter;

        if (parameter.GetSpriteList().Length == 0) return;
        _spriteList = parameter.GetSpriteList();

        _curIdx = 0;
        _curIconImage.sprite = _spriteList[_curIdx];

        _prevButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(_spriteList.Length != 1);
        
    }

    public void ShowPrevImage()
    {
        _curIdx--;
        if (_curIdx < 0) _curIdx = 0;
        SetCurIconImage();
    }

    public void ShowNextImage()
    {
        _curIdx++;
        if (_curIdx >= _spriteList.Length) _curIdx = _spriteList.Length - 1;
        SetCurIconImage();
    }

    private void SetCurIconImage()
    {
        _curIconImage.sprite = _spriteList[_curIdx];
        _prevButton.gameObject.SetActive(_curIdx > 0);
        _nextButton.gameObject.SetActive(_curIdx < _spriteList.Length - 1);
    }
}
