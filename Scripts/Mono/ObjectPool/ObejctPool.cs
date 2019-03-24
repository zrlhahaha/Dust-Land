using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int Count { get { return pool.Count; } }
    public PoolMember prefab;
    private Stack<PoolMember> pool;

    private void Awake()
    {
        pool = new Stack<PoolMember>();
    }

    public T GetPoolMember<T>(T prefab) where T : PoolMember
    {
        if (prefab == null)
        {
            Debug.LogError("object pool prefab is null");
            return null;
        }

        GameObject obj = prefab.gameObject;
        if (!obj.IsPrefab())
            Debug.LogError("argument is supposed to be a prefab");

        if (typeof(T) == typeof(PoolMember))
            Debug.LogWarning("Don't use PoolMember as prefab,it won't call recycle automaticly");

        if (pool.Count == 0)
            Instantiate();

        PoolMember go = pool.Pop();
        go.gameObject.SetActive(true);

        return (T)go;
    }

    public void ReturnPool<T>(T poolMember) where T : PoolMember
    {
        poolMember.transform.SetParent(transform);
        poolMember.gameObject.SetActive(false);
        pool.Push(poolMember);
    }

    public void Initialize(int amount)
    {
        for (int i = 0; i < amount; i++)
            Instantiate();
    }

    public void Instantiate()
    {
        if (prefab == null)
        {
            Debug.LogError("object pool prefab is null");
            return;
        }

        PoolMember go = Instantiate(prefab);
        go.gameObject.SetActive(false);
        go.transform.SetParent(transform);
        go.SetPool(this);
        pool.Push(go);
    }
}