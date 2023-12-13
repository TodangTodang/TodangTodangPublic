using System.Collections.Generic;
using UnityEngine;

public class DecoStoreMapSetter : MonoBehaviour
{
    private Dictionary<int, GameObject> _decoObjectDic;
    private DecoStoreData _decoStoreData;
    private GameManager _gameManager;

    private void Awake()
    {
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_decoStoreData == null) _decoStoreData = _gameManager.GetDecoStore();

        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
        Debug.Assert(_decoStoreData != null, "Null Exception : DecoStoreData");

        _decoObjectDic = new Dictionary<int, GameObject>();

        Transform[] childTransforms = transform.GetComponentsInChildren<Transform>();
        Debug.Assert(childTransforms != null, "Null Exception : childTransforms");

        foreach (Transform child in childTransforms)
        {
            string childName = child.name;

            int num;
            if (int.TryParse(childName.Split('_')[0], out num))
            {
                _decoObjectDic.Add(num, child.gameObject);
            }
        }
    }

    private void Start()
    {
        if (_decoStoreData != null)
        {
            _decoStoreData.OnDecorationBought += HandleDecorationBought;
        }

        Init();
    }

    private void Init()
    {
        List<StoreDecorationInfoData> storeDecorationInfoDatas = _decoStoreData.GetAllStoreDecoData();

        Debug.Assert(storeDecorationInfoDatas != null, "Null Exception : storeDecorationInfoDatas");

        foreach (var data in storeDecorationInfoDatas)
        {
            HandleDecorationBought(data.DefaultData.ID);
        }
    }

    private void HandleDecorationBought(int id)
    {
        StoreDecorationInfoData data = _decoStoreData.GetStoreDecoData(id);

        Debug.Assert(data != null, "Null Exception : StoreDecorationInfoData");

        if (_decoObjectDic.TryGetValue(id, out GameObject obj))
        {
            obj.gameObject.SetActive(data.IsSoldOut);
        }
    }

    private void OnDestroy()
    {
        _decoObjectDic.Clear();
    }
}
