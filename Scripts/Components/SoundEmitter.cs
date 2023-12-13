using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public void PlaySFX(string name)
    {
        SoundManager.Instance.Play(name);        
    }
}
