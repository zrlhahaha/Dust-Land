using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMember : MonoBehaviour {
    
    ObjectPool pool;

    public void ReturnPool()
    {
        if (pool == null)
        {
            Debug.LogWarning(transform.name + " Something wrong with original pool");
            return;
        }
        Reset();
        pool.ReturnPool(this);
    }

    public void SetPool(ObjectPool pool)
    {
        this.pool = pool; 
    }

    public virtual void Reset()
    {

    }
}
