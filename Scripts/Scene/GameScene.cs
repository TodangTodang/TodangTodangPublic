using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] private GameObject[] MapPrefabs;

    [Header("플레이어")]
    [SerializeField] private GameObject PlayerPrefabs;
    [SerializeField] private Vector3 _playerSpawnPos;

    private UI_VirtualPad _virtualPad;

    protected override bool Init()
    {
        if (!base.Init()) return false;

        SceneType = Scenes.GameScene;

        for (int i = 0; i < MapPrefabs.Length; i++)
        {
            Instantiate(MapPrefabs[i]);    
        }

        _virtualPad = UIManager.Instance.GetUIComponent<UI_VirtualPad>();

        if (PlayerPrefabs != null) Instantiate(PlayerPrefabs, _playerSpawnPos, Quaternion.Euler(0,180,0));
        else Debug.LogError("PlayerPrefabs Inspector 설정 누락");

        SoundManager.Instance.Play(Strings.Sounds.BGM_GAMESCENE, Enums.AudioType.Bgm); 

        return true;
    }

    public override void Clear()
    {
        base.Clear();
        PoolManager.Instance.Clear();
    }
}
