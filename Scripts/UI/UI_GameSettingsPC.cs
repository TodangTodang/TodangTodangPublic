using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSettingsPC : UI_GameSettings
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _screenModeButton;
    [SerializeField] private TextMeshProUGUI _screenModeText;

    private List<Resolution> _resolutions = new List<Resolution>();
    private int _resolutionNum;
    private bool _isFullScreenMode = true; 
    private Resolution _defaultResolution;

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
                if (resolution.refreshRateRatio.value >= 59 && resolution.refreshRateRatio.value <= 60)
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
            }

            optionNum++;
        }

        _resolutionDropdown.RefreshShownValue();

        _screenModeButton.isOn =_isFullScreenMode ? true : false;

        _defaultResolution = new Resolution
        {
            width = Screen.currentResolution.width,
            height = Screen.currentResolution.height,
        };

        InitBind();
    }

    public override void OpenUI(bool isSound = true, bool isAnimated = true)
    {
        base.OpenUI(isSound, isAnimated);
        _defaultResolution.width = Screen.currentResolution.width;
        _defaultResolution.height = Screen.currentResolution.height;
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

        GameSettingsData.MarkAsDirty();
    }

    public void OnFullScreenButton(bool isFull)
    {
        _isFullScreenMode = isFull;

        _screenModeText.text = isFull ? "전체화면" : "창모드";
        Screen.SetResolution(_resolutions[_resolutionNum].width, _resolutions[_resolutionNum].height,_isFullScreenMode);
        GameSettingsData.MarkAsDirty();
    }

    protected override void RevertData()
    {
        base.RevertData();


        int defaultResolutionIndex = -1;
        for (int i = 0; i < _resolutions.Count; i++)
        {
            if (_resolutions[i].width == _defaultResolution.width && _resolutions[i].height == _defaultResolution.height)
            {
                defaultResolutionIndex = i;
                break;
            }
        }

        if (defaultResolutionIndex != -1)
        {
            _resolutionDropdown.value = defaultResolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            Screen.SetResolution(_resolutions[defaultResolutionIndex].width, _resolutions[defaultResolutionIndex].height, _isFullScreenMode);
        }
    }
}

