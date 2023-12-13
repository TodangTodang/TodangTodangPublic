using UnityEngine;

public class CustomerEmotion : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _happyParticle;
    [SerializeField] private ParticleSystem[] _angryParticle;

    public void PlayHappy()
    {
#if UNITY_EDITOR
        DebugUtil.AssertNotAllocateInInspector(_happyParticle!=null,nameof(_happyParticle));
#endif
        
        for (int i = 0; i < _happyParticle.Length; ++i)
        {
            _happyParticle[i].Play();   
        }
    }

    public void PlayAngry()
    {
#if UNITY_EDITOR
        DebugUtil.AssertNotAllocateInInspector(_angryParticle!=null,nameof(_angryParticle));
#endif
        
        for (int i = 0; i < _angryParticle.Length; ++i)
        {
            _angryParticle[i].Play();
        }
    }
}
