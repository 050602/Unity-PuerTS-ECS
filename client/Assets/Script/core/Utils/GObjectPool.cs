using System.Collections.Generic;
using FairyGUI;

public delegate bool GObjectPoolGetConditionFn(GObject go);

/// <summary>
/// GObject 专用对象池
/// </summary>
public class GObjectPool
{
    private List<GObject> _pool = new List<GObject>();
    public int Count { get; private set; }
    // public delegate void CallBack(GObject go);
    // private CallBack _putCb;
    // private CallBack _getCb;
    public GObjectPool()
    {
        Count = 0;
    }
    public void Put(GObject go)
    {
        if (go.parent != null)
        {
            go.parent.RemoveChild(go);
        }
        GObject found = _pool.Find((GObject go2) =>
        {
            return go2 == go;
        });
        if (found == null)
        {
            _pool.Add(go);
            Count++;
            // if (_putCb != null)
            // {
            //     _putCb(go);
            // }
        }
    }
    // public void SetPutCb(CallBack cb)
    // {
    //     _putCb = cb;
    // }
    public GObject Get(GObjectPoolGetConditionFn cond = null)
    {
        if (Count > 0)
        {
            if (cond == null)
            {
                GObject go = _pool[0];
                _pool.RemoveAt(0);
                Count--;
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
                        _pool.RemoveAt(0);
                        Count--;
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
            GObject go = _pool[0];
            if (go.parent != null)
            {
                go.parent.RemoveChild(go);
            }
            go.Dispose();
            _pool.RemoveAt(0);
        }
        Count = 0;
    }

}
