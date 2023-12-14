using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UI_GameSettings : UI_Base
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _effectSlider;

    [SerializeField] private Button _textureLeftButton;
    [SerializeField] private Button _textureRightButton;
    [SerializeField] private TextMeshProUGUI _textureText;

    [SerializeField] private Button _framerateLeftButton;
    [SerializeField] private Button _framerateRightButton;
    [SerializeField] private TextMeshProUGUI _framerateText;

    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _dataDeleteButton;
    [SerializeField] private Button _closedButton;

    protected SoundManager _soundManager;
    private DataManager _dataManager;
    private UIManager _uiManager;
    private SceneManagerEx _sceneManagerEx;

    protected GameSettingsData GameSettingsData;
    protected GameSettingsData DefaultSettingsData;

    private List<int> _framerateList;

    protected virtual void Awake()
    {
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_dataManager == null) _dataManager = DataManager.Instance;
        if (_uiManager == null) _uiManager = UIManager.Instance;
        if (_sceneManagerEx == null) _sceneManagerEx = SceneManagerEx.Instance;

        if (GameSettingsData == null) GameSettingsData = _dataManager.LoadGameSettings();
        if (DefaultSettingsData == null)
            DefaultSettingsData = GameSettingsData.Copy();

        Debug.Assert(_soundManager != null, "Null Exception : SoundManager");
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_sceneManagerEx != null, "Null Exception : SceneManagerEx");

        _framerateList = new List<int>() { 30, 60 };

        InitBind();
    }

    protected virtual void InitBind()
    {
        _bgmSlider.onValueChanged.AddListener(OnBGMVolumChanged);
        _effectSlider.onValueChanged.AddListener(OnEffectVolumchanged);

        _textureLeftButton.onClick.AddListener(() => OnTextureChange(-1));
        _textureRightButton.onClick.AddListener(() => OnTextureChange(1));

        _framerateLeftButton.onClick.AddListener(() => OnFramerateChange(-1));
        _framerateRightButton.onClick.AddListener(() => OnFramerateChange(1));

        _exitButton.onClick.AddListener(OnExitButton);
        _dataDeleteButton.onClick.AddListener(OnDataResetButton);
        _closedButton.onClick.AddListener(OnClosedButton);
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);

        GameSettingsData = _dataManager.LoadGameSettings();
        DefaultSettingsData = GameSettingsData.Copy();

        LoadGameSettings();
    }

    public void LoadGameSettings()
    {
        OnBGMVolumChanged(GameSettingsData.BgmVolume);
        OnEffectVolumchanged(GameSettingsData.EffectVolume);

        OnTextureChange(0);
        OnFramerateChange(0);
    }

    private void OnBGMVolumChanged(float value)
    {
        _bgmSlider.value = value;
        _soundManager.SetVolume(value, Enums.AudioType.Bgm);
        GameSettingsData.BgmVolume = value;
    }

    private void OnEffectVolumchanged(float value)
    {
        _effectSlider.value = value;
        _soundManager.SetVolume(value, Enums.AudioType.Effect);
        GameSettingsData.EffectVolume = value;
    }

    private void OnTextureChange(int value)
    {
        GameSettingsData.TextureQuality += value;
        GameSettingsData.TextureQuality = (TextureQuality)Mathf.Clamp((float)GameSettingsData.TextureQuality, 0, (int)TextureQuality.High);

        switch (GameSettingsData.TextureQuality)
        {
            case TextureQuality.Low:
                _textureText.text = Strings.GameSettings.LOW;
                break;
            case TextureQuality.Medium:
                _textureText.text = Strings.GameSettings.MEDIUM;
                break;
            case TextureQuality.High:
                _textureText.text = Strings.GameSettings.HIGH;
                break;
        }

        _textureLeftButton.interactable = GameSettingsData.TextureQuality != 0;
        _textureRightButton.interactable = (int)GameSettingsData.TextureQuality != 2;

        _soundManager.Play(Strings.Sounds.UI_BUTTON);

        _dataManager.SaveGameSettings(GameSettingsData);
    }

    private void OnFramerateChange(int value)
    {
        int currentFramerateIndex = _framerateList.IndexOf(GameSettingsData.Framerate);
        int newIndex = Mathf.Clamp(currentFramerateIndex + value, 0, _framerateList.Count - 1);
        GameSettingsData.Framerate = _framerateList[newIndex];

        _framerateText.text = GameSettingsData.Framerate.ToString();
        _framerateRightButton.interactable = newIndex < _framerateList.Count - 1;
        _framerateLeftButton.interactable = newIndex > 0;

        _soundManager.Play(Strings.Sounds.UI_BUTTON);

        _dataManager.SaveGameSettings(GameSettingsData);
    }

    private void OnDataResetButton()
    {
        _uiManager.ShowPopup<UI_DefaultPopup>(
          new PopupParameter(
              content: Strings.GameSettings.DATARESETBUTTON
          , confirmCallback: OnDataResetConfirmButon
          , cancelCallback: null
              )
          );
    }

    private void OnDataResetConfirmButon()
    {
        _dataManager.DeleteDataAll();

        CloseUI();
        SceneManagerEx.Instance.LoadScene(Scenes.LoadingScene, Scenes.StartScene);
    }

    private void OnExitButton()
    {
        UIManager.Instance.ShowPopup<UI_DefaultPopup>(
            new PopupParameter(
                content: Strings.GameSettings.EXITBUTTON
                , confirmCallback: EndGame
                , cancelCallback: null
                )
            );
    }

    private void EndGame()
    {
        Application.Quit();
    }

    public void OnClosedButton()
    {
        if (HasChange())
        {
            _uiManager.ShowPopup<UI_DefaultPopup>(
           new PopupParameter(
               content: Strings.GameSettings.CLOSEDBUTTON
           , confirmCallback: OnApplyConfirmButon
           , cancelCallback: OnCancelButton
               )
           );
        }
        else
        {
            CloseUI();
        }
    }

    private bool HasChange()
    {
        return !GameSettingsData.Equals(DefaultSettingsData); 
    }

    private void OnApplyConfirmButon()
    {
        ApplyGraphics();
        _dataManager.SaveGameSettings(GameSettingsData);
        CloseUI();
    }

    private void ApplyGraphics()
    {
        QualitySettings.globalTextureMipmapLimit = (int)GameSettingsData.TextureQuality;
        Application.targetFrameRate = GameSettingsData.Framerate;
    }

    private void OnCancelButton()
    {
        RevertData();
        CloseUI();
    }

    protected virtual void RevertData()
    {
        GameSettingsData = DefaultSettingsData;
        _dataManager.SaveGameSettings(GameSettingsData);
        LoadGameSettings();
    }
}
