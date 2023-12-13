
using UnityEngine;

[CreateAssetMenu]
public class AchievementNewsSO : NewsSO
{
    [SerializeField] private int _starAchievement;

    public int StarAchievement => _starAchievement;
}
