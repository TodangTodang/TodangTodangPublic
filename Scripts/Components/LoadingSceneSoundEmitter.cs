
using UnityEngine;

public class LoadingSceneSoundEmitter : MonoBehaviour
{
    public void PlayFootStep()
    {
        SoundManager soundManager = SoundManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(soundManager,nameof(soundManager));
#endif
    
        soundManager.Play(Strings.Sounds.PLAYER_FOOTSTEP+'1');
    }

    public void PlayFallDown()
    {
        SoundManager soundManager = SoundManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(soundManager,nameof(soundManager));
#endif
    
        soundManager.Play(Strings.Sounds.PLAYER_FALLDOWN);
    }
}
