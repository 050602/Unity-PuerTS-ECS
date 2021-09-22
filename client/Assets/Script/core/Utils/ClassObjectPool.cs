using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPoolClaer
{
    void clearPool();
}

public class ClassObjectPool<T> : IPoolClaer where T : class, ObjectPoolIT, new()
{
    protected Stack<T> pool = new Stack<T>();
    //最大对象个数  <=0表示不限制个数
    protected int maxCount;
    //没有别回收的个数
    protected int noRecycleCount;
    public ClassObjectPool(int maxCount)
    {
        this.maxCount = maxCount;
        for (int i = 0; i < maxCount; i++)
        {
            pool.Push(new T());
        }
    }

    public void clearPool()
    {
        this.pool.Clear();

        Debug.Log("清理对象池:" + this.pool.Count);
    }

    public T getObject(bool createIfPoolEmpty = true)
    {
        if (maxCount > 0)
        {
            T rtn = pool.Pop();
            if (rtn == null)
            {
                if (createIfPoolEmpty)
                {
                    rtn = new T();
                }
            }
            noRecycleCount++;
            return rtn;
        }
        else
        {
            if (createIfPoolEmpty)
            {
                T rtn = new T();
                noRecycleCount++;
                return rtn;
            }
        }
        return null;
    }

    /// <summary>
    /// 回收类对象
    /// </summary>
    public bool Recycle(T obj)
    {
        if (obj == null)
            return false;
        noRecycleCount--;
        if (pool.Count >= maxCount && maxCount > 0)
        {
            obj = null;
            return false;
        }
        obj.recycleToObjectPool();
        pool.Push(obj);
        return true;
    }
}