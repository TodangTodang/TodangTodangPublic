public class AnimationSoundEmitter : SoundEmitter
{
    public void PlayFootStep()
    {
        PlaySFX(Strings.Sounds.PLAYER_FOOTSTEP+'1');
    }

    public void PlayFallDown()
    {
        PlaySFX(Strings.Sounds.PLAYER_FALLDOWN);
    }
}
