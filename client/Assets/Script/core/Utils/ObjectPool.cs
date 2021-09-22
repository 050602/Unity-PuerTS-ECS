using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ObjectPoolIT
{
    void recycleToObjectPool();
}
public class ObjectPool : Singleton<ObjectPool>
{
    #region 类对象池的使用
    protected Dictionary<Type, object> classObjectPoolDic = new Dictionary<Type, object>();
    /// <summary>
    /// 创建类对象池，创建完成后在外面可以保存ClassObjectPool<T>，然后调用Spawn和Recycle来创建和回收类对象
    /// </summary>
    public ClassObjectPool<T> GetOrCreateClassObjectPool<T>(int maxCount = 0) where T : class, ObjectPoolIT, new()
    {
        Type type = typeof(T);
        object obj = null;
        if (!classObjectPoolDic.TryGetValue(type, out obj) || obj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
            classObjectPoolDic.Add(type, newPool);
            return newPool;
        }
        return obj as ClassObjectPool<T>;
    }
    #endregion

    public void clear()
    {

        foreach (var item in classObjectPoolDic)
        {
            var obj = item.Value as IPoolClaer;
            obj.clearPool();
        }

    }

    public T getObj<T>() where T : class, ObjectPoolIT, new()
    {
        var pool = this.GetOrCreateClassObjectPool<T>();
        var obj = pool.getObject();
        return obj;
    }

    public bool recycle<T>(T obj) where T : class, ObjectPoolIT, new()
    {
        var pool = this.GetOrCreateClassObjectPool<T>();
        return pool.Recycle(obj);
    }

}