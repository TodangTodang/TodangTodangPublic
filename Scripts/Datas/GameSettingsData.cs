public class GameSettingsData
{
    public float BgmVolume = 0.5f;
    public float EffectVolume = 0.5f;
    public Enums.TextureQuality TextureQuality = Enums.TextureQuality.Medium;
    public int Framerate = 60;

    private bool _isDirty = false; 
    public bool IsDirty => _isDirty;

    public GameSettingsData(float bgmVolume, float effectVolume,Enums.TextureQuality textureQuality, int framerate) : base()
    {
        BgmVolume = bgmVolume;
        EffectVolume = effectVolume;
        TextureQuality = textureQuality;
        Framerate = framerate;
    }

    public void MarkAsDirty()
    {
        _isDirty = true; 
    }

    public void ResetDirty()
    {
        _isDirty = false;
    }
}
