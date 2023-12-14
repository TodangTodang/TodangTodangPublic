using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NewsSO : ScriptableObject
{
    [SerializeField] private int _day;
    [SerializeField][TextArea(3, 5)] private string _headLine;
    [SerializeField][TextArea(3, 5)] private string _contents;
    [SerializeField] private List<EffectSO> _effects;
    [SerializeField] private List<int> _effectDuration;

    public int Day => _day;
    public string HeadLine => _headLine;
    public string Contents => _contents;
    public List<EffectSO> Effects => _effects;
    public List<int> EffectDuration => _effectDuration;
}
