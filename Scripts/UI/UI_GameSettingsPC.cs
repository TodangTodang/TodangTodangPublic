using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSettingsPC : UI_GameSettings
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _screenModeButton;
    [SerializeField] private TextMeshProUGUI _screenModeText;

    private List<Resolution> _resolutions = new List<Resolution>();
    private int _resolutionNum;
    private bool _isFullScreenMode;

    protected override void Awake()
    {
        base.Awake();

        _resolutions.Clear();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];

            if (_resolutions.Any(existingResolution => existingResolution.width == resolution.width))
            {
                continue;
            }

            float aspectRatio = (float)resolution.width / resolution.height;
            if (Mathf.Approximately(aspectRatio, 16f / 10f) || Mathf.Approximately(aspectRatio, 16f / 9f) || Mathf.Approximately(aspectRatio, 4f / 3f))
            {
                if (resolution.refreshRateRatio.value >= 59 && resolution.refreshRateRatio.value < 61)
                {
                    _resolutions.Add(resolution);
                }
            }
        }

        _resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution resolution in _resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolution.width + "X" + resolution.height;
            _resolutionDropdown.options.Add(option);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                _resolutionDropdown.value = optionNum;
                option.text = resolution.width + "X" + resolution.height;
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); 
            }

            optionNum++;
        }

        _resolutionDropdown.RefreshShownValue();

        _isFullScreenMode = Screen.fullScreen;
        GameSettingsData.ScreenMode = _isFullScreenMode;
        DefaultSettingsData.ScreenMode = _isFullScreenMode; 
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, true);
        DefaultSettingsData.ResolutionData  = _resolutions[_resolutionNum];

        GameSettingsData.ResolutionData = _resolutions[_resolutionNum]; 

         _isFullScreenMode = Screen.fullScreen;
        DefaultSettingsData.ScreenMode = _isFullScreenMode;
        GameSettingsData.ScreenMode = _isFullScreenMode; 

        Screen.SetResolution(_resolutions[_resolutionNum].width, _resolutions[_resolutionNum].height, _isFullScreenMode);

        _screenModeButton.isOn = _isFullScreenMode ? true : false;
    }

    protected override void InitBind()
    {
        base.InitBind();
        _screenModeButton.onValueChanged.RemoveListener(OnFullScreenButton);

        _resolutionDropdown.onValueChanged.AddListener(OnDropboxOptionChange);
        _screenModeButton.onValueChanged.AddListener(OnFullScreenButton);
    }

    private void OnDropboxOptionChange(int x)
    {
        _resolutionNum = x;

        Resolution[] resolutionsArray = new Resolution[_resolutions.Count];
        _resolutions.CopyTo(resolutionsArray);
        Screen.SetResolution(resolutionsArray[_resolutionNum].width, resolutionsArray[_resolutionNum].height, _isFullScreenMode);

        PlayerPrefs.SetInt(Strings.Prefs.SAVESCREEN, 1);

        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        GameSettingsData.ResolutionData = _resolutions[_resolutionNum];
    }

    public void OnFullScreenButton(bool isFull)
    {
        _isFullScreenMode = isFull;

        _screenModeText.text = _isFullScreenMode ? Strings.GameSettings.FULL_SCREENMODE : Strings.GameSettings.WINDOW_SCREENMODE;
        Screen.SetResolution(_resolutions[_resolutionNum].width, _resolutions[_resolutionNum].height, _isFullScreenMode);
        _soundManager.Play(Strings.Sounds.UI_BUTTON);
        GameSettingsData.ScreenMode = _isFullScreenMode; 
    }

    protected override void RevertData()
    {
        base.RevertData();

        int defaultResolutionIndex = -1;
        for (int i = 0; i < _resolutions.Count; i++)
        {
            if (_resolutions[i].width == DefaultSettingsData.ResolutionData.width && _resolutions[i].height == DefaultSettingsData.ResolutionData.height)
            {
                defaultResolutionIndex = i;
                break;
            }
        }

        if (defaultResolutionIndex != -1)
        {
            _resolutionDropdown.value = defaultResolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            _isFullScreenMode = DefaultSettingsData.ScreenMode;
            _screenModeText.text = _isFullScreenMode ? Strings.GameSettings.FULL_SCREENMODE : Strings.GameSettings.WINDOW_SCREENMODE;
            Screen.SetResolution(DefaultSettingsData.ResolutionData.width, DefaultSettingsData.ResolutionData.height, _isFullScreenMode);
        }
    }
}

