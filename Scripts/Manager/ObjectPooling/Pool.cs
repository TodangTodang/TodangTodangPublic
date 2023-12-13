using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private GameObject _original;
    private Transform _root;

    private Queue<Poolable> _poolQueue = new Queue<Poolable>(20);

    // Pool 초기화 - 기본 오브젝트 생성 개수 3개
    public void Init(GameObject go, int count = 3)
    {
        _original = go;
        _root = transform;

        for (int i = 0; i < count; i++)
        {
            Push(Create());
        }
    }

    Poolable Create()
    {
        GameObject go = _original;
        go.SetActive(false);
        go = Instantiate(go);
        go.name = _original.name;
        return go.GetOrAddComponent<Poolable>();
    }

    // 오브젝트 넣기
    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        poolable.transform.SetParent(_root);
        poolable.gameObject.SetActive(false);

        _poolQueue.Enqueue(poolable);
    }

    // 오브젝트 빼기
    public Poolable Pop(Vector3 pos, Quaternion q, Transform parent)
    {
        Poolable poolable;

        if (_poolQueue.Count > 0)
            poolable = _poolQueue.Dequeue();
        else
            poolable = Create();

        // 부모 Transform이 지정되었다면 변경
        // 지정되지 않았다면 현재 Pool의 root를 부모로 설정
        if (parent == null) poolable.transform.SetParent(_root);
        else poolable.transform.SetParent(parent);

        poolable.transform.SetPositionAndRotation(pos, q);
        poolable.gameObject.SetActive(true);

        return poolable;
    }
}
