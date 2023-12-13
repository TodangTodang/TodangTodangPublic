using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    // Resources/Sprites 폴더 안의 이미지 소스들 저장
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    // Resources/Prefabs 폴더 안의 프리팹들 저장
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    public Sprite LoadSprite(string name)
    {
        if (!sprites.ContainsKey(name))
        {
            // 이미지 소스가 요청 시점에 딕셔너리에 존재하지 않는다면 Load
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{name}");
            if (!sprite)
            {
#if UNITY_EDITOR
                Debug.LogError($"Sprite 로드 실패 : {name}");
#endif
                return null;
            }
            sprites.Add(name, sprite);
        }
        return sprites[name];
    }

    public GameObject Instantiate(string name, Transform parent = null)
    {
        if (!prefabs.ContainsKey(name))
        {
            GameObject go = Resources.Load<GameObject>($"Prefabs/{name}");
            if (!go)
            {
#if UNITY_EDITOR
                Debug.LogError($"Prefab 로드 실패 : {name}");
#endif
                return null;
            }
            prefabs.Add(name, go);
        }

        GameObject prefab = prefabs[name];
        if (prefab.TryGetComponent<Poolable>(out Poolable p))
        {
            PoolManager poolManager = PoolManager.Instance;
            if (poolManager)
            {
                return PoolManager.Instance.Pop(prefab, name, prefab.transform.position, prefab.transform.rotation, parent).gameObject;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"Null Exception : PoolManager");
#endif
                return null;
            }
        }

        return Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        if (go.TryGetComponent<Poolable>(out Poolable p))
        {
            PoolManager.Instance.Push(p);
            go = null;
            return;
        }

        Object.Destroy(go);
        go = null;
    }
}
