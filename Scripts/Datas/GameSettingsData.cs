using UnityEngine;

public class GameSettingsData
{
    public float BgmVolume = 0.5f;
    public float EffectVolume = 0.5f;
    public Enums.TextureQuality TextureQuality = Enums.TextureQuality.Medium;
    public int Framerate = 60;
    public Resolution ResolutionData = Screen.currentResolution; 
    public bool ScreenMode  = Screen.fullScreen;


    public GameSettingsData(float bgmVolume, float effectVolume,Enums.TextureQuality textureQuality, int framerate) : base()
    {
        BgmVolume = bgmVolume;
        EffectVolume = effectVolume;
        TextureQuality = textureQuality;
        Framerate = framerate;
    }

    public GameSettingsData Copy()
    {
        return new GameSettingsData(BgmVolume, EffectVolume, TextureQuality, Framerate);
    }

    public bool Equals(GameSettingsData other)
    {
        if (other == null) return false;

        return BgmVolume == other.BgmVolume &&
               EffectVolume == other.EffectVolume &&
               TextureQuality == other.TextureQuality &&
               Framerate == other.Framerate &&
               ResolutionData.width == other.ResolutionData.width &&
               ResolutionData.height == other.ResolutionData.height&&
               ScreenMode == other.ScreenMode ;
    }
}
