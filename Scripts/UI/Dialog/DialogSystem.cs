using System.Collections;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [SerializeField][TextArea(3, 5)] private string[] _dialogs;
    [SerializeField] private float _typingSpeed = 0.07f;
    [SerializeField] private bool _autoClose;

    private int _curIdx = -1;
    private bool _isTyping = false;
    private bool _isNextClicked = false;
    private UI_Dialog _uiDialog;

    private SoundManager _soundManager; 

    public void Init()
    {
        if(_soundManager == null)   _soundManager = SoundManager.Instance;
        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");

        _uiDialog = UIManager.Instance.GetUIComponent<UI_Dialog>();
        _uiDialog.OnClicked += CallOnClickNextBtn;
        SetNextDialog();
    }

    public bool UpdateDialog()
    {
        if (_isNextClicked)
        {
            if (_isTyping)
            {
                _isTyping = false;
                StopCoroutine(nameof(TypingText));
                _uiDialog.SetDialogText(_dialogs[_curIdx]);
                _isNextClicked = false;
                return false;
            }

            if (_curIdx + 1 < _dialogs.Length) SetNextDialog();
            else
            {
                if (_autoClose) _uiDialog.CloseUI(false);
                _isNextClicked = false;
                return true;
            }
            _isNextClicked = false;
        }

        return false;
    }

    private void CallOnClickNextBtn()
    {
        _isNextClicked = true;
    }

    private void SetNextDialog()
    {
        _curIdx++;
        _uiDialog.OpenUI(false);
        StartCoroutine(nameof(TypingText));
    }

    private IEnumerator TypingText()
    {
        int idx = 0;
        _isTyping = true;

        while (idx <= _dialogs[_curIdx].Length)
        {
            _uiDialog.SetDialogText(_dialogs[_curIdx].Substring(0, idx));
            idx++;

            if (idx % 2 == 0)
            {
                if (!_soundManager.IsPlaying(Strings.Sounds.UI_DIALOG))
                    _soundManager.Play(Strings.Sounds.UI_DIALOG);
            }

            yield return new WaitForSecondsRealtime(_typingSpeed);
        }

        _isTyping = false;
        if (_soundManager.IsPlaying(Strings.Sounds.UI_DIALOG))
            _soundManager.Stop(Strings.Sounds.UI_DIALOG); 
    }
}
