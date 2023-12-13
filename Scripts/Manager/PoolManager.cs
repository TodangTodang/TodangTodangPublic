using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 전체 오브젝트 풀들을 관리
    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    // 새로운 풀 생성
    public void CreatePool(GameObject go, int count = 3)
    {
        GameObject poolObj= new GameObject();
        poolObj.name = $"{go.name}_Pool";
        Pool pool = poolObj.AddComponent<Pool>();
        pool.Init(go, count);

        _pool.Add(go.name, pool);
    }

    // 해당하는 풀에 오브젝트 넣기
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        // 해당 오브젝트 풀이 존재하지 않는 경우 그냥 파괴
        if (!_pool.ContainsKey(name))
        {
            Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    // 해당하는 풀에서 오브젝트 빼기
    public Poolable Pop(GameObject go, string path, Vector3 pos, Quaternion q, Transform parent)
    {
        // 해당 오브젝트 풀이 존재하지 않는 경우 새로 생성
        if (!_pool.ContainsKey(go.name))
            CreatePool(go);

        return _pool[go.name].Pop(pos, q, parent);
    }

    public void Clear()
    {
        _pool.Clear();
    }
}
