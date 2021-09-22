using System.Collections.Generic;
using UnityEngine;

public delegate bool GameObjectPoolGetConditionFn(GameObject go);

/// <summary>
/// GameObject 专用对象池
/// </summary>
public class GameObjectPool
{
    private int noneLayer;
    private List<GameObject> _pool = new List<GameObject>();
    private List<int> _layerList = new List<int>();
    public int Count { get; private set; }
    // public delegate void CallBack(GameObject go);
    // private CallBack _putCb;
    // private CallBack _getCb;

    public GameObjectPool()
    {
        noneLayer = LayerMask.NameToLayer("None");
        Count = 0;
    }
    public void Put(GameObject go)
    {
        // go.transform.SetParent(container.transform);
        // _layerList.push();
        for (int n = _pool.Count - 1; n >= 0; n--)
        {
            if (_pool[n] == go)
            {
                return;
            }
        }
        // var cs = go.GetComponents<Component>();
        // for (int n = cs.Length-1; n >=0; n--)
        // {
        // }
        go.SetActive(false);
        // go.layer = noneLayer;
        _pool.Add(go);
        _layerList.Add(go.layer);
        Count++;
        // if (_putCb != null)
        // {
        //     _putCb(go);
        // }
    }
    // public void SetPutCb(CallBack cb)
    // {
    //     _putCb = cb;
    // }
    public GameObject Get(GameObjectPoolGetConditionFn cond = null)
    {
        if (Count > 0)
        {
            if (cond == null)
            {
                var go = _pool[0];
                var layer = _layerList[0];
                _pool.RemoveAt(0);
                _layerList.RemoveAt(0);
                Count--;
                go.SetActive(true);
                // go.layer = layer;
                // if (_putCb != null)
                // {
                //     _getCb(go);
                // }
                return go;
            }
            else
            {
                for (int n = _pool.Count - 1; n >= 0; n--)
                {
                    var go = _pool[n];
                    if (cond(go))
                    {
                        var layer = _layerList[n];
                        _pool.RemoveAt(n);
                        _layerList.RemoveAt(n);
                        Count--;
                        go.SetActive(true);
                        // go.layer = layer;
                        // if (_putCb != null)
                        // {
                        //     _getCb(go);
                        // }
                        return go;
                    }
                }
            }
        }
        return null;
    }
    // public void SetGetCb(CallBack cb)
    // {
    //     _getCb = cb;
    // }
    public void Clear()
    {
        while (_pool.Count > 0)
        {
            GameObject go = _pool[0];
            GameObject.Destroy(go);
            _pool.RemoveAt(0);
        }
        Count = 0;
    }

}
