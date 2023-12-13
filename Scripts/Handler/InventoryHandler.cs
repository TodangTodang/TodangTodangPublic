using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] private InventoryController[] _inventoryControllers;
    [SerializeField] private UI_Inventory _uiInventory;

    private Enums.InventoryType _curSelectedInventoryType;
    private InventoryController _curController;

    private void Start()
    {
        _uiInventory.OnOpenInventory += CallOnOpenInventory;
        _uiInventory.OnTabChanged += CallOnChangeTab;
        _uiInventory.OnDetailViewChanged += CallOnRefreshDetail;
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _inventoryControllers.Length; i++) _inventoryControllers[i].InitDatas();
        _curController = _inventoryControllers[(int)Enums.InventoryType.Ingredient];
        _curController.RefreshTab();
        _curController.RefreshPlayerMoney(true);
    }

    private void CallOnOpenInventory()
    {
        Init();
    }

    private void CallOnChangeTab(Enums.InventoryType inventoryType)
    {
        _curSelectedInventoryType = inventoryType;
        _curController = _inventoryControllers[(int)_curSelectedInventoryType];
        _curController.RefreshTab();
    }

    private void CallOnRefreshDetail(int idx)
    {
        _curController.RefreshDetail(idx);
    }
}
