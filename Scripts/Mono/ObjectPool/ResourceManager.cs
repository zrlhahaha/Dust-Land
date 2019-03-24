using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitEffect
{
    Defualt = 0,
    Concrete = 1,
    Metal = 2,
}


public class ResourceManager:MonoBehaviour{

    public List<PoolMember> prefabs = new List<PoolMember>();
    public List<int> defaultAmount = new List<int>();
    public Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();
    //索引要和enum一一对应
    public FX[] hitEffects;

    public static ResourceManager _instance; 
    
    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("There is another instance of ObjectPool");

        _instance = this;

        for (int i = 0; i < prefabs.Count; i++)
        {
            AddPool(prefabs[i], defaultAmount[i]);
        }
    }

    public T GetPoolMember<T>(T prefab) where T:PoolMember
    {
        if (prefab == null)
        {
            Debug.LogWarning("prefab passed in is null");
            return null;
        }

        GameObject obj = prefab.gameObject;
        if (!obj.IsPrefab())
            Debug.LogError("argument is supposed to be a prefab");

        if (typeof(T) ==typeof(PoolMember))
            Debug.LogWarning("Don't use PoolMember as prefab,it won't call recycle automaticly");

        if (!pools.ContainsKey(obj))
        {
            Debug.LogWarning("prefab name:" + obj.name + " Pool is not found,better add a new pool in editor");
            AddPool(prefab, 1);
        }

        ObjectPool pool = pools[obj];

        PoolMember go = pool.GetPoolMember(prefab);
        go.gameObject.SetActive(true);
        return (T)go;
    }

    public FX GetHitFX(HitEffect hitFxType)
    {
        int index = (int)hitFxType;

        if (hitEffects == null)
        {
            Debug.LogError("hitEffects is null");
            return null;
        }

        if (hitEffects.Length < index)
        {
            Debug.LogError("ResourceManager.GetHitFX: argument is out of range");
            return null;
        }

        FX prefab = hitEffects[index];
        return GetPoolMember(prefab);
    }

    public void AddPool(PoolMember prefab,int initialAmount)
    {
        if (prefab == null)
            Debug.LogError("prefab passed in is null");

        if (!prefab.gameObject.IsPrefab())
        {
            Debug.LogError(prefab.name + " is not a prefab");
            return;
        }
        //创建对象池们
        GameObject go = new GameObject(prefab.gameObject.name);
        ObjectPool pool = go.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        go.transform.SetParent(transform); ;

        pools.Add(prefab.gameObject, pool);
        pool.Initialize(initialAmount);
    }
}
