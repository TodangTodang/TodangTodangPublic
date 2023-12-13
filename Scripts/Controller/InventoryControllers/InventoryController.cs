using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] protected UI_Inventory _uiInventory;
    [SerializeField] protected UI_InventoryDetail _uiInventoryDetail;

    protected int _curSelectedItemIdx;
    protected UI_InventorySlot[] _slotList;

    protected GameManager _gameManager;
    protected UIManager _uiManager;
    protected DataManager _dataManager;
    protected ResourceManager _resourceManager;
    protected SoundManager _soundManager;
    protected PlayerData _playerData;

    private void Start()
    {
#if UNITY_EDITOR
        Debug.Assert(_uiInventory != null, "Null Exception : UI_Inventory");
#endif
    }

    public virtual void InitDatas()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
        _dataManager = DataManager.Instance;
        _resourceManager = ResourceManager.Instance;
        _soundManager = SoundManager.Instance;
        _playerData = _gameManager.GetPlayerData();

        #region Check Null Exception
#if UNITY_EDITOR
        Debug.Assert(_gameManager, "Null Exception : GameManager");
        Debug.Assert(_uiManager, "Null Exception : GameManager");
        Debug.Assert(_dataManager, "Null Exception : DataManager");
        Debug.Assert(_resourceManager, "Null Exception : ResourceManager");
        Debug.Assert(_soundManager, "Null Exception : SoundManager");
        Debug.Assert(_playerData != null, "Null Exception : PlayerData");
#endif
        #endregion
    }

    public void RefreshPlayerMoney(bool isInit)
    {
        _uiInventory.UpdatePlayerMoneyUI(_playerData.Money, isInit);
    }

    public virtual void RefreshTab()
    {
        _curSelectedItemIdx = 0;
    }

    public virtual void RefreshDetail(int idx)
    {
        _curSelectedItemIdx = idx;
    }
}
